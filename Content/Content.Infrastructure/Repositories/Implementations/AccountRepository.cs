using Content.Core.Entities;
using Content.Infrastructure.Data;
using Content.Infrastructure.Repositories.Implementations.Common;
using Content.Infrastructure.Repositories.Interfaces;

namespace Content.Infrastructure.Repositories.Implementations;

public class AccountRepository : GenericRepository<Account, ContentContext>, IAccountRepository
{
    public AccountRepository(ContentContext context) : base(context)
    {
    }
}