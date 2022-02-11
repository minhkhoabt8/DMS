namespace MessageContracts;

public class FileVersionReady
{
    public FileVersionReady(int id)
    {
        ID = id;
    }

    public int ID { get; set; }
}