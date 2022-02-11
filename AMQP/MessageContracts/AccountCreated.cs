namespace MessageContracts;

public class AccountCreated
{
    public Guid ID { get; set; }
    public string Username { get; set; }
    public string FullName { get; set; }
}