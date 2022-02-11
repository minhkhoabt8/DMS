namespace Auth.Core.Entities;

public class Folder
{
    public Guid ID { get; set; }
    public Guid OwnerID { get; set; }
    public Account Owner { get; set; }
    public Guid? ParentFolderID { get; set; }
    public Folder ParentFolder { get; set; }
    public ICollection<Folder> SubFolders { get; set; }
    public ICollection<File> Files { get; set; }
    public ICollection<FolderShareRule> ShareRules { get; set; }
}