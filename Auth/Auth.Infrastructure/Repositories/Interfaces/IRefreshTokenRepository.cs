using System.Threading.Tasks;
using Auth.Core.Entities;
using Auth.Infrastructure.Repositories.Interfaces.Common;

namespace Auth.Infrastructure.Repositories.Interfaces;

public interface IRefreshTokenRepository :
    IAddAsync<RefreshToken>
{
    Task<RefreshToken?> FindByTokenAsync(string token);
    Task<RefreshToken?> FindByTokenIncludeAccountAsync(string? token);
}