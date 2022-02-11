using Metadata.Infrastructure.Data;
using Metadata.Infrastructure.DTOs.File;
using Metadata.Infrastructure.Repositories.Implementations.Common;
using Metadata.Infrastructure.Repositories.Interfaces;
using Metadata.Infrastructure.Repositories.QueryExtensions;
using Microsoft.EntityFrameworkCore;
using File = Metadata.Core.Entities.File;

namespace Metadata.Infrastructure.Repositories.Implementations;

public class FileRepository : GenericRepository<File, MetadataContext>, IFileRepository
{
    public FileRepository(MetadataContext context) : base(context)
    {
    }

    public Task<File?> QuerySingleAsync(FileQuerySingle query, bool trackChanges = false)
    {
        IQueryable<File> files = _context.Files;

        if (!trackChanges)
        {
            files = files.AsNoTracking();
        }

        if (query.ID.HasValue)
        {
            files = files.Where(fd => fd.ID == query.ID);
        }

        if (query.OwnerID.HasValue)
        {
            files = files.Where(fd => fd.OwnerID == query.OwnerID);
        }

        if (query.ParentFolderID.HasValue)
        {
            files = files.Where(fd => fd.ParentFolderID == query.ParentFolderID);
        }

        if (!string.IsNullOrEmpty(query.SearchText))
        {
            files = files.Where(f => f.Name.ToLower().Contains(query.SearchText.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(query.Include))
        {
            files = files.IncludeDynamic(query.Include);
        }

        return files.SingleOrDefaultAsync();
    }
}