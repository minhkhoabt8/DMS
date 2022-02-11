namespace Metadata.Infrastructure.DTOs.Folder;

public class FolderContentItem
{
    public Guid ID { get; set; }
    public Guid? ParentFolderID { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public long? Size { get; set; }
    public bool IsUploading { get; set; }
    public Guid OwnerID { get; set; }
}