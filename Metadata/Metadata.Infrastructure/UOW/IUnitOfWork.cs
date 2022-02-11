using Metadata.Infrastructure.Repositories.Interfaces;

namespace Metadata.Infrastructure.UOW;

public interface IUnitOfWork
{
    public IAccountRepository AccountRepository { get; }
    public IFolderRepository FolderRepository { get; }
    public IFileRepository FileRepository { get; }
    public IFileVersionRepository FileVersionRepository { get; }
    public ITagRepository TagRepository { get; }
    Task<int> CommitAsync();
}