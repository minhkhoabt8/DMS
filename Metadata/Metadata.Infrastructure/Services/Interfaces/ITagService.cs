using Metadata.Infrastructure.DTOs.Tag;

namespace Metadata.Infrastructure.Services.Interfaces;

public interface ITagService
{
     Task<IEnumerable<TagReadDTO>> GetAllAsync();
     Task<TagReadDTO> CreateAsync(TagWriteDTO dto);
     Task<TagReadDTO> UpdateAsync(int id, TagWriteDTO dto);
     Task DeleteAsync(int id);
}