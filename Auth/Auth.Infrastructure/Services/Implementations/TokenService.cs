using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Auth.Core.Entities;
using Auth.Infrastructure.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Infrastructure.Services.Implementations;

public class TokenService: ITokenService
{
    private readonly IRoleService _roleService;

    public TokenService(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public RefreshToken GenerateRefreshToken(Account account)
    {
        var randomBytes = RandomNumberGenerator.GetBytes(64);

        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(randomBytes),
            // Last for two months
            Expires = DateTime.UtcNow.AddDays(60),
            AccountID = account.ID
        };

        return refreshToken;
    }

    public async Task<string> GenerateTokenAsync(Account account, string secret, string issuer)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));

        var claims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier, account.ID.ToString())
        };

        foreach (var role in await _roleService.GetAccountRolesAsync(account.ID))
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Name));
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(4),
            Issuer = issuer,
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}