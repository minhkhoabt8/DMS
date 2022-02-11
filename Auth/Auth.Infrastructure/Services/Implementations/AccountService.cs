using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Auth.Core.Entities;
using Auth.Core.Exceptions;
using Auth.Infrastructure.DTOs.Account;
using Auth.Infrastructure.DTOs.Common;
using Auth.Infrastructure.Repositories.Interfaces;
using Auth.Infrastructure.Services.Interfaces;
using AutoMapper;

namespace Auth.Infrastructure.Services.Implementations;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IMapper _mapper;

    public AccountService(IAccountRepository accountRepository, IMapper mapper)
    {
        _accountRepository = accountRepository;
        _mapper = mapper;
    }

    public async Task<AccountReadDTO> GetAccountAsync(Guid id)
    {
        var account = await _accountRepository.FindIncludeRolesAsync(id);

        if (account == null)
        {
            throw new EntityWithIDNotFoundException<Account>(id);
        }

        return _mapper.Map<AccountReadDTO>(account);
    }

    public async Task<IEnumerable<AccountReadDTO>> GetAllAccountsAsync()
    {
        var accounts = await _accountRepository.GetAllAsync();

        return _mapper.Map<IEnumerable<AccountReadDTO>>(accounts);
    }

    public async Task<PaginatedResponse<AccountReadDTO>> QueryAccountsAsync(AccountQuery query)
    {
        var accounts = await _accountRepository.QueryAsync(query);

        return PaginatedResponse<AccountReadDTO>.FromEnumerableWithMapping(accounts, query, _mapper);
    }
}