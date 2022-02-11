using System;
using System.Linq;
using System.Threading.Tasks;
using Auth.Core.Entities;
using Auth.Core.Exceptions;
using Auth.Infrastructure.Mapper;
using Auth.Infrastructure.Repositories.Interfaces;
using Auth.Infrastructure.Services.Implementations;
using AutoMapper;
using Moq;
using Xunit;

namespace Auth.Tests.Services;

public class AccountServiceTests
{
    [Fact]
    public void GetAccountAsync_Throws_EntityNotFoundException_When_Account_Not_Exists()
    {
        var accountID = Guid.Empty;
        var accountRepository = new Mock<IAccountRepository>();
        accountRepository.Setup(ar => ar.FindIncludeRolesAsync(accountID)).ReturnsAsync((Account?) null);
        var accountService = new AccountService(accountRepository.Object, new Mock<IMapper>().Object);

        Assert.ThrowsAsync<EntityWithIDNotFoundException<Account>>(() => accountService.GetAccountAsync(accountID));
    }

    [Fact]
    public async Task GetAccountAsync_Returns_AccountReadDTO_When_Account_Exists()
    {
        var account = CreateTestAccount();
        var accountRepository = new Mock<IAccountRepository>();
        accountRepository.Setup(ar => ar.FindIncludeRolesAsync(account.ID)).ReturnsAsync(account);
        var accountService = new AccountService(accountRepository.Object, CreateMapper());

        var accountDTO = await accountService.GetAccountAsync(account.ID);

        Assert.NotNull(accountDTO);
        Assert.Equal(account.ID, accountDTO.ID);
    }

    [Fact]
    public async Task GetAllAccountsAsync_Returns_All_Accounts()
    {
        var allAccounts = new[] {CreateTestAccount(), CreateTestAccount()};
        var accountRepository = new Mock<IAccountRepository>();
        accountRepository.Setup(ar => ar.GetAllAsync(false)).ReturnsAsync(allAccounts);
        var accountService = new AccountService(accountRepository.Object, CreateMapper());

        var accountDTOs = await accountService.GetAllAccountsAsync();

        Assert.Equal(allAccounts.Length, accountDTOs.Count());
        Assert.Equal(allAccounts.Select(a => a.ID), accountDTOs.Select(a => a.ID));
    }

    private Account CreateTestAccount()
    {
        return Account.Create(Guid.NewGuid(), "", "", "", "");
    }

    private IMapper CreateMapper()
    {
        return new Mapper(new MapperConfiguration(e => e.AddProfile(new MappingProfile())));
    }
}