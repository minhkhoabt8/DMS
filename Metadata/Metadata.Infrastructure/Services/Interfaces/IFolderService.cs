using Metadata.Infrastructure.DTOs.Folder;

namespace Metadata.Infrastructure.Services.Interfaces;

public interface IFolderService
{
    Task<FolderReadDTO> CreateAsync(FolderCreateDTO dto);
    Task<FolderReadDTO> UpdateAsync(Guid id, FolderUpdateDTO dto);
    Task<FolderReadDTO> GetDetailsAsync(Guid id);
    Task<IEnumerable<FolderContentItem>> GetContentsAsync(Guid id);
    Task<IEnumerable<FolderContentItem>> GetRootContentsAsync();
    Task<IEnumerable<FolderContentItem>> GetSharedContentsAsync();
    Task<IEnumerable<FolderBreadcrumbDTO>> GetPathAsync(Guid id);
}