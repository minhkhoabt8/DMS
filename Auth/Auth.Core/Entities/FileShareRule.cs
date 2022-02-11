using Auth.Core.Enums;

namespace Auth.Core.Entities;

///<inheritdoc cref="FolderShareRule"/>
public class FileShareRule
{
    public int ID { get; set; }
    public Guid FileID { get; set; }
    public File File { get; set; }
    public ShareType Type { get; set; }
    public ShareScope Scope { get; set; }
    public bool IsRoot { get; set; }
    public string Value { get; set; }
    public DateTime ExpirationDate { get; set; }
}