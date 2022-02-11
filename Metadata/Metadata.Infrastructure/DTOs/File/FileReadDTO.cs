namespace Metadata.Infrastructure.DTOs.File;

public class FileReadDTO
{
    public Guid ID { get; set; }
    public string Name { get; set; }
    public long Size { get; set; }
    public string Type { get; set; }
    public Guid OwnerID { get; set; }
    public Guid? ParentFolderID { get; set; }

    // public ICollection<Tag> Tags { get; set; }
    // public ICollection<FileEvent> Events { get; set; }
    public DateTime LastModified { get; set; }
}