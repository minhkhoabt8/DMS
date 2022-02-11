using Auth.Core.Entities;
using Auth.Infrastructure.Repositories.Interfaces.Common;

namespace Auth.Infrastructure.Repositories.Interfaces;

public interface IFolderRepository :
    IFindAsync<Folder>,
    IAddAsync<Folder>
{
}