using System.Threading.Tasks;

namespace Auth.Infrastructure.Repositories.Interfaces.Common;

public interface IFindAsync<T> where T : class
{
    Task<T?> FindAsync(params object[] keys);
}