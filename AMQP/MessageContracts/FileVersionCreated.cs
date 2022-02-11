namespace MessageContracts;

public class FileVersionCreated
{
    public int ID { get; set; }
    public Guid FileID { get; set; }
    public long VersionNumber { get; set; }
    public long Size { get; set; }
    public string Type { get; set; }
}