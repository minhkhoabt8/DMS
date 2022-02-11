using Metadata.Infrastructure.DTOs.File;

namespace Metadata.Infrastructure.Services.Interfaces;

public interface IFileService
{
    Task<FileReadDTO> GetFileDetails(Guid id);
    Task<FileReadDTO> CreateFileAsync(FileCreateDTO dto);
    Task<FileReadDTO> UpdateFileAsync(Guid id, FileUpdateDTO dto);
}