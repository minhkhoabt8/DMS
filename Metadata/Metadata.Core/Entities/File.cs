using System.ComponentModel.DataAnnotations;
using Metadata.Core.Entities.Common;
using Metadata.Core.Enums;
using Metadata.Core.Events.Integration;
using Metadata.Core.Extensions;

namespace Metadata.Core.Entities;

/// <summary>
/// A resource in system
/// </summary>
/// <remarks>Resource can be folder or file</remarks>
public class File : EntityWithEvents, ITrackLastModified
{
    private File()
    {
    }

    public static File Create(string name, Guid parentFolderID)
    {
        var file = new File
        {
            Name = name,
            ParentFolderID = parentFolderID
        };

        file.Events.Add(new FilePostCreatedEvent(file));

        return file;
    }

    public Guid ID { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string Name { get; set; }

    public long Size => CurrentVersion?.Size ?? 0;
    public FileType Type => CurrentVersion?.Type ?? FileType.Unknown;
    public bool IsUploading => !CurrentVersion?.IsReady ?? false;

    public DateTime CreatedAt { get; set; } = DateTime.Now.SetKindUtc();

    /// <summary>
    /// The type of file
    /// </summary>
    /// <remarks>When a file is first created, it's type will be unknown until a FileVersion is uploaded, which will determine the file type</remarks>

    public Guid OwnerID { get; set; }

    public Account Owner { get; set; }
    public Guid? ParentFolderID { get; set; }
    public Folder ParentFolder { get; set; }
    public ICollection<Tag> Tags { get; set; }
    public ICollection<FileEvent> FileEvents { get; set; }
    public ICollection<FileVersion>? Versions { get; set; }
    public DateTime LastModified { get; set; }


    private FileVersion? CurrentVersion
    {
        get { return Versions?.Where(v => v.IsActive)?.OrderByDescending(v => v.VersionNumber)?.FirstOrDefault(); }
    }
}