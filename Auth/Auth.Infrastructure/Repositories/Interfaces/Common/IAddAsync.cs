using System.Threading.Tasks;

namespace Auth.Infrastructure.Repositories.Interfaces.Common;

public interface IAddAsync<in T> where T : class
{
    Task AddAsync(T obj);
}