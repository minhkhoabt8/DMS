using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auth.Infrastructure.Repositories.Interfaces.Common;

public interface IQueryAsync<TEntity, in TQuery>
{
    Task<IEnumerable<TEntity>> QueryAsync(TQuery query, bool trackChanges = false);
}