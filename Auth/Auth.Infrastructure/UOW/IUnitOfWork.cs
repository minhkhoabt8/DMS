using System.Threading.Tasks;
using Auth.Infrastructure.Repositories.Interfaces;

namespace Auth.Infrastructure.UOW;

public interface IUnitOfWork
{
    IRoleRepository RoleRepository { get; }
    IAccountRepository AccountRepository { get; }
    IRefreshTokenRepository RefreshTokenRepository { get; }
    IFileRepository FileRepository { get; }
    IFolderRepository FolderRepository { get; }
    Task<int> CommitAsync();
}