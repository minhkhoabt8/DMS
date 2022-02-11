using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Auth.Infrastructure.DTOs;
using Auth.Infrastructure.DTOs.Account;
using Auth.Infrastructure.DTOs.Common;

namespace Auth.Infrastructure.Services.Interfaces;

public interface IAccountService
{
    Task<AccountReadDTO> GetAccountAsync(Guid id);
    Task<IEnumerable<AccountReadDTO>> GetAllAccountsAsync();
    Task<PaginatedResponse<AccountReadDTO>> QueryAccountsAsync(AccountQuery query);
}