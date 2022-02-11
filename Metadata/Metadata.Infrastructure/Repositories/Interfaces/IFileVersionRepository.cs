using Metadata.Core.Entities;
using Metadata.Infrastructure.Repositories.Interfaces.Common;

namespace Metadata.Infrastructure.Repositories.Interfaces;

public interface IFileVersionRepository :
    IFindAsync<FileVersion>,
    IAddAsync<FileVersion>
{
}