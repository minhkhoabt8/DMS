namespace MessageContracts;

public class FileVersionDeactivated
{
    public FileVersionDeactivated(int id)
    {
        ID = id;
    }

    public int ID { get; set; }
}