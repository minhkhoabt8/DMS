using Metadata.Infrastructure.Data;
using Metadata.Infrastructure.Repositories.Interfaces;

namespace Metadata.Infrastructure.UOW;

public class UnitOfWork : IUnitOfWork
{
    private readonly MetadataContext _context;
    private readonly IServiceProvider _serviceProvider;
    private readonly IDictionary<string, object> _singletonRepositories;

    public UnitOfWork(MetadataContext context, IServiceProvider serviceProvider)
    {
        _context = context;
        _serviceProvider = serviceProvider;
        _singletonRepositories = new Dictionary<string, object>();
    }

    public IAccountRepository AccountRepository => GetSingletonRepository<IAccountRepository>();
    public IFolderRepository FolderRepository => GetSingletonRepository<IFolderRepository>();
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