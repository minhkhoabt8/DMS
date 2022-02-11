namespace Metadata.Infrastructure.Repositories.Interfaces.Common;

public interface IDelete<in T> where T : class
{
    void Delete(T obj);
}