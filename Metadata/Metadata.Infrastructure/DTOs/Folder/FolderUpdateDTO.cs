using System.ComponentModel.DataAnnotations;

namespace Metadata.Infrastructure.DTOs.Folder;

public class FolderUpdateDTO
{
    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string Name { get; set; }
}