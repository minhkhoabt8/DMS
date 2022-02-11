namespace Metadata.Infrastructure.DTOs.Folder;

public class FolderReadDTO
{
    public Guid ID { get; set; }
    public string Name { get; set; }
    public Guid OwnerID { get; set; }
    public string OwnerName { get; set; }
    public Guid? ParentFolderID { get; set; }
    public DateTime LastModified { get; set; }
}