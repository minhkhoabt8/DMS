using Metadata.Core.Extensions;

namespace Metadata.Core.Entities;

public class FolderEvent
{
    public int ID { get; set; }
    public Guid FolderID { get; set; }
    public Folder Folder { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now.SetKindUtc();
}