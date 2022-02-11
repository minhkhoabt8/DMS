using Metadata.Core.Entities;
using Metadata.Infrastructure.Data;
using Metadata.Infrastructure.Repositories.Implementations.Common;
using Metadata.Infrastructure.Repositories.Interfaces;

namespace Metadata.Infrastructure.Repositories.Implementations;

public class FileVersionRepository : GenericRepository<FileVersion, MetadataContext>, IFileVersionRepository
{
    public FileVersionRepository(MetadataContext context) : base(context)
    {
    }
}