namespace Content.Core.Entities;

/// <summary>
/// A resource in system
/// </summary>
/// <remarks>Resource can be folder or file</remarks>
public class File
{
    public Guid ID { get; set; }
    public Guid OwnerID { get; set; }
    public Account Owner { get; set; }
    public ICollection<FileVersion> Versions { get; set; }

    public FileVersion? GetNewestVersion(bool? ready = null, bool? active = null)
    {
        return Versions
            .Where(v => !ready.HasValue || v.IsActive == ready)
            .Where(v => !active.HasValue || v.IsReady == active)
            .OrderByDescending(v => v.VersionNumber)
            .FirstOrDefault();
    }
}