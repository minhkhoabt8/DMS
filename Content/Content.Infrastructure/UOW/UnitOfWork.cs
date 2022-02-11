using Content.Infrastructure.Data;
using Content.Infrastructure.Repositories.Interfaces;

namespace Content.Infrastructure.UOW;

public class UnitOfWork : IUnitOfWork
{
    private readonly ContentContext _context;
    private readonly IServiceProvider _serviceProvider;
    private readonly IDictionary<string, object> _singletonRepositories;

    public UnitOfWork(ContentContext context, IServiceProvider serviceProvider)
    {
        _context = context;
        _serviceProvider = serviceProvider;
        _singletonRepositories = new Dictionary<string, object>();
    }

    public IAccountRepository AccountRepository => GetSingletonRepository<IAccountRepository>();

    public IFileRepository FileRepository => GetSingletonRepository<IFileRepository>();
    public IFileVersionRepository FileVersionRepository => GetSingletonRepository<IFileVersionRepository>();

    public Task<int> CommitAsync()
    {
        return _context.SaveChangesAsync();
    }

    private T GetSingletonRepository<T>()
    {
        if (!_singletonRepositories.ContainsKey(typeof(T).Name))
        {
            _singletonRepositories[typeof(T).Name] =
                _serviceProvider.GetService(typeof(T)) ?? throw new InvalidOperationException();
        }

        return (T) _singletonRepositories[typeof(T).Name];
    }
}