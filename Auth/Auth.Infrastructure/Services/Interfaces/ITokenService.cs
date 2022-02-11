using System.Threading.Tasks;
using Auth.Core.Entities;

namespace Auth.Infrastructure.Services.Interfaces;

public interface ITokenService
{
    Task<string> GenerateTokenAsync(Account account, string secret, string issuer);
    RefreshToken GenerateRefreshToken(Account account);
}