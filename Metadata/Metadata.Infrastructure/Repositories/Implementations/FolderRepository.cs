using Metadata.Core.Entities;
using Metadata.Infrastructure.Data;
using Metadata.Infrastructure.DTOs.Folder;
using Metadata.Infrastructure.Repositories.Implementations.Common;
using Metadata.Infrastructure.Repositories.Interfaces;
using Metadata.Infrastructure.Repositories.QueryExtensions;
using Microsoft.EntityFrameworkCore;

namespace Metadata.Infrastructure.Repositories.Implementations;

public class FolderRepository : GenericRepository<Folder, MetadataContext>, IFolderRepository
{
    public FolderRepository(MetadataContext context) : base(context)
    {
    }

    public Task<Folder?> QuerySingleAsync(FolderQuerySingle query, bool trackChanges = false)
    {
        IQueryable<Folder> folders = _context.Folders;

        if (!trackChanges)
        {
            folders = folders.AsNoTracking();
        }

        if (query.ID.HasValue)
        {
            folders = folders.Where(fd => fd.ID == query.ID);
        }

        if (query.IsRoot.HasValue)
        {
            folders = folders.Where(fd => fd.IsRoot == query.IsRoot);
        }

        if (query.OwnerID.HasValue)
        {
            folders = folders.Where(fd => fd.OwnerID == query.OwnerID);
        }

        if (query.ParentFolderID.HasValue)
        {
            folders = folders.Where(fd => fd.ParentFolderID == query.ParentFolderID);
        }

        if (!string.IsNullOrEmpty(query.SearchText))
        {
            folders = folders.Where(fd => fd.Name.ToLower().Contains(query.SearchText.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(query.Include))
        {
            folders = folders.IncludeDynamic(query.Include);
        }

        return folders.SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<Folder>> QueryAsync(FolderQuery query, bool trackChanges = false)
    {
        IQueryable<Folder> folders = _context.Folders;

        if (!trackChanges)
        {
            folders = folders.AsNoTracking();
        }

        if (query.AccountID.HasValue)
        {
            folders = folders.Where(fd => fd.OwnerID == query.AccountID);
        }

        if (query.ParentFolderID.HasValue)
        {
            folders = folders.Where(fd => fd.ParentFolderID == query.ParentFolderID);
        }

        if (!string.IsNullOrEmpty(query.SearchText))
        {
            folders = folders.Where(fd => fd.Name.ToLower().Contains(query.SearchText.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(query.Include))
        {
            folders = folders.IncludeDynamic(query.Include);
        }

        return await folders.ToListAsync();
    }
}