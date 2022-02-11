using System.ComponentModel.DataAnnotations;

namespace Metadata.Core.Entities;

/// <summary>
/// Represents a tag used to organize resources
/// </summary>
public class Tag
{
    public int ID { get; set; }

    [Required]
    [StringLength(30, MinimumLength = 1)]
    public string Name { get; set; }

    public ICollection<File> Files { get; set; }
}