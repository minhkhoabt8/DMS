using System.ComponentModel.DataAnnotations;

namespace Metadata.Infrastructure.DTOs.File;

public class FileUpdateDTO
{
    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string Name { get; set; }
}