using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Core.Entities;
using Auth.Infrastructure.Data;
using Auth.Infrastructure.DTOs;
using Auth.Infrastructure.DTOs.Account;
using Auth.Infrastructure.Repositories.Implementations.Common;
using Auth.Infrastructure.Repositories.Interfaces;
using Auth.Infrastructure.Repositories.QueryExtensions;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Repositories.Implementations;

public class AccountRepository : GenericRepository<Account, AuthContext>, IAccountRepository
{
    public AccountRepository(AuthContext context) : base(context)
    {
    }


    public async Task<IEnumerable<Account>> QueryAsync(AccountQuery query, bool trackChanges = false)
    {
        IQueryable<Account> accounts = _context.Accounts.AllowInactive(query.ShowInactive);

        if (!trackChanges)
        {
            accounts = accounts.AsNoTracking();
        }

        if (!string.IsNullOrWhiteSpace(query.Include))
        {
            accounts = accounts.IncludeDynamic(query.Include);
        }

        if (!string.IsNullOrEmpty(query.SearchText))
        {
            accounts = accounts.FilterAndOrderByTextSimilarity(query.SearchText);
        }

        return await Task.FromResult(accounts.ToList());
    }

    public Task<Account?> FindIncludeRolesAsync(Guid id)
    {
        return _context.Accounts.Include(acc => acc.Roles).FirstOrDefaultAsync(acc => acc.ID == id);
    }
}