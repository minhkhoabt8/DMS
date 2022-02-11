using Content.Infrastructure.Data;
using Content.Infrastructure.DTOs.File;
using Content.Infrastructure.Repositories.Implementations.Common;
using Content.Infrastructure.Repositories.Interfaces;
using Content.Infrastructure.Repositories.QueryExtensions;
using Microsoft.EntityFrameworkCore;
using File = Content.Core.Entities.File;

namespace Content.Infrastructure.Repositories.Implementations;

public class FileRepository : GenericRepository<File, ContentContext>, IFileRepository
{
    public FileRepository(ContentContext context) : base(context)
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

        if (!string.IsNullOrWhiteSpace(query.Include))
        {
            files = files.IncludeDynamic(query.Include);
        }

        return files.SingleOrDefaultAsync();
    }
}