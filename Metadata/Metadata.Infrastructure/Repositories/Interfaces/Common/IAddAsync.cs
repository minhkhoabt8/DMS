namespace Metadata.Infrastructure.Repositories.Interfaces.Common;

public interface IAddAsync<in T> where T : class
{
    Task AddAsync(T obj);
}