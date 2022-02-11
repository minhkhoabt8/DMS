using Auth.Core.Entities.Common;
using Auth.Core.Events.Integration;

namespace Auth.Core.Entities;

/// <summary>
/// Represents an account
/// </summary>
public class Account : EntityWithEvents, ITextSearchableEntity, ISoftDeleteEntity
{
    private Account()
    {
    }

    public static Account Create(Guid id, string email, string phone, string username, string fullname)
    {
        var account = new Account
        {
            ID = id,
            Email = email,
            Phone = phone,
            Username = username,
            FullName = fullname
        };

        account.Events.Add(new AccountPostCreatedEvent(account));

        return account;
    }

    public Guid ID { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Username { get; set; }
    public string FullName { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<Role> Roles { get; set; }

    public IReadOnlyDictionary<Func<string>, double> SearchTextsWithWeights => new Dictionary<Func<string>, double>
    {
        {() => FullName, .5},
        {() => Username, .35},
        {() => Email, .15}
    };
}