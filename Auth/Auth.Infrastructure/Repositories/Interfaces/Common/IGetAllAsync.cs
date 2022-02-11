using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auth.Infrastructure.Repositories.Interfaces.Common;

public interface IGetAllAsync<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(bool trackChanges = false);
}