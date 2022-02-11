using System;
using Auth.Core.Entities;
using Auth.Core.Events.Integration;
using Xunit;

namespace Auth.Tests.Entities;

public class AccountTests
{
    [Fact]
    public void Create_Account_Emits_AccountPostCreatedEvent()
    {
        var account = Account.Create(Guid.Empty, "", "", "", "");

        Assert.Single(account.Events, e => e is AccountPostCreatedEvent te && te.Account == account);
    }
}