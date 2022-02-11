using Metadata.Core.Extensions;

namespace Metadata.Core.Entities;

public class FileEvent
{
    public int ID { get; set; }
    public Guid FileID { get; set; }
    public File File { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now.SetKindUtc();
}