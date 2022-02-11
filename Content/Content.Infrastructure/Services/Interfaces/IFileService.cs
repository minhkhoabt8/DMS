using Content.Infrastructure.DTOs.File;
using Content.Infrastructure.DTOs.FileVersion;

namespace Content.Infrastructure.Services.Interfaces;

public interface IFileService
{
    Task<IEnumerable<FileVersionReadDTO>> GetVersionsAsync(Guid id);
    Task DeactivateVersionAsync(Guid id, int versionID);
    Task<byte[]> GetContentAsync(Guid id, int? versionID);
    Task UploadAsync(Guid id, FileUploadDTO dto);
}