using Metadata.Infrastructure.DTOs.File;
using Metadata.Infrastructure.Repositories.Interfaces.Common;
using File = Metadata.Core.Entities.File;

namespace Metadata.Infrastructure.Repositories.Interfaces;

public interface IFileRepository :
    IFindAsync<File>,
    IAddAsync<File>,
    IDelete<File>,
    IQuerySingleAsync<File, FileQuerySingle>
{
}