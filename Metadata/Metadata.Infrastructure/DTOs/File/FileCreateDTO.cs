using System.ComponentModel.DataAnnotations;

namespace Metadata.Infrastructure.DTOs.File;

public class FileCreateDTO
{
    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string Name { get; set; }

    public Guid? ParentFolderID { get; set; }
}