namespace MessageContracts;

public class FileCreated
{
    public Guid ID { get; set; }
    public string Name { get; set; }
    public Guid OwnerID { get; set; }
    public Guid ParentFolderID { get; set; }
}