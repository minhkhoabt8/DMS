using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Auth.Core.Entities;
using Auth.Infrastructure.Repositories.Interfaces.Common;

namespace Auth.Infrastructure.Repositories.Interfaces;

public interface IRoleRepository :
    IGetAllAsync<Role>,
    IFindAsync<Role>,
    IAddAsync<Role>
{
    Task<Role?> FindByNameIgnoreCaseAsync(string roleName);
    Task<IEnumerable<Role>> GetAccountRolesAsync(Guid accountID);
}