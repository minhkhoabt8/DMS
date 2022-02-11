using Content.Infrastructure.DTOs.File;
using Content.Infrastructure.Repositories.Interfaces.Common;
using File = Content.Core.Entities.File;

namespace Content.Infrastructure.Repositories.Interfaces;

public interface IFileRepository :
    IFindAsync<File>,
    IAddAsync<File>,
    IDelete<File>,
    IQuerySingleAsync<File, FileQuerySingle>
{
}