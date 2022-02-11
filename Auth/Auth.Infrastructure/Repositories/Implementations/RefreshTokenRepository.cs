using System.Threading.Tasks;
using Auth.Core.Entities;
using Auth.Infrastructure.Data;
using Auth.Infrastructure.Repositories.Implementations.Common;
using Auth.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Repositories.Implementations;

public class RefreshTokenRepository : GenericRepository<RefreshToken, AuthContext>, IRefreshTokenRepository
{
    public RefreshTokenRepository(AuthContext context) : base(context)
    {
    }

    public Task<RefreshToken?> FindByTokenAsync(string token)
    {
        return _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == token);
    }

    public Task<RefreshToken?> FindByTokenIncludeAccountAsync(string? token)
    {
        return _context.RefreshTokens.Include(rt => rt.Account).FirstOrDefaultAsync(rt => rt.Token == token);
    }
}