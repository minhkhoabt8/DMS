using System;
using System.Threading.Tasks;
using Auth.Core.Entities;
using Auth.Infrastructure.DTOs;
using Auth.Infrastructure.DTOs.Account;
using Auth.Infrastructure.Repositories.Interfaces.Common;

namespace Auth.Infrastructure.Repositories.Interfaces;

public interface IAccountRepository :
    IAddAsync<Account>,
    IGetAllAsync<Account>,
    IFindAsync<Account>,
    IQueryAsync<Account, AccountQuery>
{
    Task<Account?> FindIncludeRolesAsync(Guid id);
}