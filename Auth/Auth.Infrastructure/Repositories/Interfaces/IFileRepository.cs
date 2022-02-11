using Auth.Infrastructure.Repositories.Interfaces.Common;
using File = Auth.Core.Entities.File;

namespace Auth.Infrastructure.Repositories.Interfaces;

public interface IFileRepository :
    IFindAsync<File>,
    IAddAsync<File>
{
}