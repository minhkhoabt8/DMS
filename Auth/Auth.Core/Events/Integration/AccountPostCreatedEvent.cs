using Auth.Core.Entities;
using Auth.Core.Events.Common;

namespace Auth.Core.Events.Integration;

public class AccountPostCreatedEvent : IntegrationEvent
{
    public AccountPostCreatedEvent(Account account)
    {
        Account = account;
    }

    public Account Account { get; }
}