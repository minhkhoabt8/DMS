using System.Threading.Tasks;
using Auth.Infrastructure.DTOs;
using Auth.Infrastructure.DTOs.Authentication;

namespace Auth.Infrastructure.Services.Interfaces;

public interface IAuthService
{
    Task<LoginOutputDTO> LoginWithUsernamePasswordAsync(LoginInputDTO inputDTO);
    Task<LoginOutputDTO> LoginWithRefreshTokenAsync(string? token);
}