using Metadata.Core.Entities;
using Metadata.Infrastructure.DTOs.Folder;
using Metadata.Infrastructure.Repositories.Interfaces.Common;

namespace Metadata.Infrastructure.Repositories.Interfaces;

public interface IFolderRepository :
    IFindAsync<Folder>,
    IQueryAsync<Folder, FolderQuery>,
    IQuerySingleAsync<Folder, FolderQuerySingle>,
    IAddAsync<Folder>,
    IDelete<Folder>
{
}