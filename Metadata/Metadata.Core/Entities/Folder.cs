using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Metadata.Core.Entities.Common;
using Metadata.Core.Events.Integration;
using Metadata.Core.Extensions;

namespace Metadata.Core.Entities;

public class Folder : EntityWithEvents, ITrackLastModified
{
    private Folder()
    {
    }

    public static Folder CreateRoot(Guid ownerID)
    {
        var folder = new Folder
        {
            Name = "root",
            IsRoot = true,
            OwnerID = ownerID
        };

        folder.Events.Add(new FolderPostCreatedEvent(folder));

        return folder;
    }

    public static Folder Create(string name, Guid? parentFolderID)
    {
        var folder = new Folder
        {
            Name = name,
            ParentFolderID = parentFolderID
        };

        folder.Events.Add(new FolderPostCreatedEvent(folder));

        return folder;
    }

    public Guid ID { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string Name { get; set; }

    public Guid OwnerID { get; set; }

    /// <summary>
    /// Whether this is the root folder of a user 
    /// </summary>
    public bool IsRoot { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.Now.SetKindUtc();
    public Account Owner { get; set; }
    public Guid? ParentFolderID { get; set; }
    [ForeignKey(nameof(ParentFolderID))] public Folder ParentFolder { get; set; }
    public ICollection<File> Files { get; set; }
    public ICollection<Folder> SubFolders { get; set; }
    public ICollection<FolderEvent> FolderEvents { get; set; }
    public DateTime LastModified { get; set; }
}