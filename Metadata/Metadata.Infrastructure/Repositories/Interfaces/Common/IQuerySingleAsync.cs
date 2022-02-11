namespace Metadata.Infrastructure.Repositories.Interfaces.Common;

public interface IQuerySingleAsync<TEntity, in TQuery>
{
    Task<TEntity?> QuerySingleAsync(TQuery query, bool trackChanges = false);
}