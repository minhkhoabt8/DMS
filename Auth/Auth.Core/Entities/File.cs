namespace Auth.Core.Entities;

public class File
{
    public Guid ID { get; set; }
    public Guid OwnerID { get; set; }
    public Account Owner { get; set; }
    public Guid ParentFolderID { get; set; }
    public Folder ParentFolder { get; set; }
    public ICollection<FileShareRule> ShareRules { get; set; }
}