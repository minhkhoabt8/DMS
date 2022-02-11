using Content.Infrastructure.Repositories.Interfaces;

namespace Content.Infrastructure.UOW;

public interface IUnitOfWork
{
    public IAccountRepository AccountRepository { get; }
    public IFileRepository FileRepository { get; }
    public IFileVersionRepository FileVersionRepository { get; }
    Task<int> CommitAsync();
}