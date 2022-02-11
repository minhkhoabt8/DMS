namespace Content.Infrastructure.Repositories.Interfaces.Common;

public interface IUpdate<in T> where T : class
{
    void Update(T obj);
}