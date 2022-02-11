using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Auth.Core.Entities;
using Auth.Infrastructure.Data;
using Auth.Infrastructure.Repositories.Implementations.Common;
using Auth.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Repositories.Implementations;

public class RoleRepository : GenericRepository<Role, AuthContext>, IRoleRepository
{
    public RoleRepository(AuthContext context) : base(context)
    {
    }

    public Task<Role?> FindByNameIgnoreCaseAsync(string roleName)
    {
        return _context.Roles.FirstOrDefaultAsync(r => r.Name.ToLower() == roleName.ToLower());
    }

    public async Task<IEnumerable<Role>> GetAccountRolesAsync(Guid accountID)
    {
        var account = await _context.Accounts.Include(acc => acc.Roles).FirstAsync(acc => acc.ID == accountID);

        return account.Roles;
    }
}