using Metadata.Core.Entities;
using Metadata.Infrastructure.Data;
using Metadata.Infrastructure.Repositories.Implementations.Common;
using Metadata.Infrastructure.Repositories.Interfaces;

namespace Metadata.Infrastructure.Repositories.Implementations;

public class AccountRepository : GenericRepository<Account, MetadataContext>, IAccountRepository
{
    public AccountRepository(MetadataContext context) : base(context)
    {
    }
}