namespace Metadata.Infrastructure.Repositories.Interfaces.Common;

public interface IGetAllAsync<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(bool trackChanges = false);
}