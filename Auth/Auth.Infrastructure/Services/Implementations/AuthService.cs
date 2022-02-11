using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Auth.Core.Entities;
using Auth.Core.Exceptions;
using Auth.Infrastructure.DTOs;
using Auth.Infrastructure.DTOs.Authentication;
using Auth.Infrastructure.Services.Interfaces;
using Auth.Infrastructure.UOW;
using Microsoft.Extensions.Configuration;

namespace Auth.Infrastructure.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly ISystemManagementService _smService;
    private readonly IRoleService _roleService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _configuration;

    public AuthService(ISystemManagementService smService, IRoleService roleService, IUnitOfWork unitOfWork,
        ITokenService tokenService, IConfiguration configuration)
    {
        _smService = smService;
        _roleService = roleService;
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _configuration = configuration;
    }

    public async Task<LoginOutputDTO> LoginWithRefreshTokenAsync(string? token)
    {
        if (string.IsNullOrEmpty(token))
        {
            throw new InvalidRefreshTokenException();
        }

        var refreshToken = (await _unitOfWork.RefreshTokenRepository.FindByTokenIncludeAccountAsync(token)) ??
                           throw new InvalidRefreshTokenException();

        // Refresh token compromised => revoke all tokens in family
        if (refreshToken.IsRevoked)
        {
            // Travel down family chain
            while (refreshToken.ReplacedByToken != null)
            {
                // Descendant token
                refreshToken =
                    await _unitOfWork.RefreshTokenRepository.FindByTokenAsync(refreshToken.ReplacedByToken);
                refreshToken!.Revoke();
            }

            await _unitOfWork.CommitAsync();
            throw new InvalidRefreshTokenException();
        }

        // Expired token
        if (refreshToken.IsExpired)
        {
            throw new InvalidRefreshTokenException();
        }

        var newRefreshToken = _tokenService.GenerateRefreshToken(refreshToken.Account);

        await _unitOfWork.RefreshTokenRepository.AddAsync(newRefreshToken);

        refreshToken.ReplaceWith(newRefreshToken);

        await _unitOfWork.CommitAsync();

        return new LoginOutputDTO
        {
            UserID = refreshToken.Account.ID.ToString(),
            Username = refreshToken.Account.Username,
            FullName = refreshToken.Account.FullName,
            DMSToken = await _tokenService.GenerateTokenAsync(refreshToken.Account, _configuration["JWT:Secret"],
                _configuration["JWT:Issuer"]),
            DMSTokenExpires = 60 * 60 * 4, // 4 hours
            RefreshToken = newRefreshToken.Token,
            RefreshTokenExpires = newRefreshToken.ExpiresIn,
            Roles = (await _roleService.GetAccountRolesAsync(refreshToken.AccountID)).Select(r => r.Name).ToArray()
        };
    }

    public async Task<LoginOutputDTO> LoginWithUsernamePasswordAsync(LoginInputDTO inputDTO)
    {
        var authResult = await _smService.LoginAsync(inputDTO);

        // Accout does not have access to library module
        if (authResult.SystemModules.All(md => md.Name != "Library"))
        {
            throw new UnauthorizedException("module Library");
        }

        var jwtToken = new JwtSecurityToken(authResult.Access_Token);

        var existingAccount = await _unitOfWork.AccountRepository.FindAsync(authResult.UserID);

        // Account not yet created in VOL Module
        if (existingAccount == null)
        {
            var newAccount = CreateAccount(authResult, jwtToken);
            await _unitOfWork.AccountRepository.AddAsync(newAccount);
            existingAccount = newAccount;
        }

        // Generate new refresh token
        var newRefreshToken = _tokenService.GenerateRefreshToken(existingAccount);

        await _unitOfWork.RefreshTokenRepository.AddAsync(newRefreshToken);

        await _unitOfWork.CommitAsync();

        return new LoginOutputDTO
        {
            UserID = authResult.UserID.ToString(),
            Username = authResult.Username,
            FullName = existingAccount.FullName,
            SysToken = authResult.Access_Token,
            SysTokenExpires = authResult.Expires_In,
            DMSToken = await _tokenService.GenerateTokenAsync(existingAccount, _configuration["JWT:Secret"],
                _configuration["JWT:Issuer"]),
            DMSTokenExpires = 60 * 60 * 4, // 4 hours
            RefreshToken = newRefreshToken.Token,
            RefreshTokenExpires = newRefreshToken.ExpiresIn,
            Roles = (await _roleService.GetAccountRolesAsync(authResult.UserID)).Select(r => r.Name).ToArray()
        };
    }

    private Account CreateAccount(SMAuthDTO authResult, JwtSecurityToken jwtToken)
    {
        var newAccount = Account.Create
        (
            authResult.UserID,
            jwtToken.Claims.First(cl => cl.Type == "Email").Value,
            jwtToken.Claims.First(cl => cl.Type == "PhoneNumber").Value,
            authResult.Username,
            jwtToken.Claims.First(cl => cl.Type == "FullName").Value
        );

        return newAccount;
    }
}