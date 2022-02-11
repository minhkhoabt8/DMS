using Metadata.Core.Enums;

namespace Metadata.Core.Entities;

public class FileVersion
{
    public int ID { get; set; }
    public Guid FileID { get; set; }
    public File File { get; set; }
    public FileType Type { get; set; }
    public long Size { get; set; }

    /// <summary>
    /// Higher means newer version
    /// </summary>
    public long VersionNumber { get; set; }

    public bool IsReady { get; set; } = false;
    public bool IsActive { get; set; } = true;
}