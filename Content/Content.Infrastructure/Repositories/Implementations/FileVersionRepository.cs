using Content.Core.Entities;
using Content.Infrastructure.Data;
using Content.Infrastructure.Repositories.Implementations.Common;
using Content.Infrastructure.Repositories.Interfaces;

namespace Content.Infrastructure.Repositories.Implementations;

public class FileVersionRepository: GenericRepository<FileVersion, ContentContext>, IFileVersionRepository
{
    public FileVersionRepository(ContentContext context) : base(context)
    {
    }
}