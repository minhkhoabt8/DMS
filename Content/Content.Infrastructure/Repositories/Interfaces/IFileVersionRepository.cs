using Content.Core.Entities;
using Content.Infrastructure.Repositories.Interfaces.Common;

namespace Content.Infrastructure.Repositories.Interfaces;

public interface IFileVersionRepository :
    IFindAsync<FileVersion>
{
}