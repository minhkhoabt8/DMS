using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Auth.Core.Entities;
using Auth.Core.Exceptions;
using Auth.Infrastructure.DTOs.Role;
using Auth.Infrastructure.Services.Implementations;
using Auth.Infrastructure.UOW;
using Moq;
using Xunit;

namespace Auth.Tests.Services;

public class RoleServiceTests
{
    [Fact]
    public async Task AssignRoleToAccountAsync_Assigns_Role_To_Account()
    {
        var account = CreateTestAccount();
        var role = new Role {ID = 1};
        account.Roles = new List<Role>();

        var mockUOW = new Mock<IUnitOfWork>();
        var roleService = new RoleService(null!, mockUOW.Object);

        mockUOW.Setup(uow => uow.RoleRepository.FindAsync(role.ID)).ReturnsAsync(role);
        mockUOW.Setup(uow => uow.AccountRepository.FindIncludeRolesAsync(account.ID)).ReturnsAsync(account);

        await roleService.AssignRoleToAccountAsync(role.ID, account.ID);

        Assert.Contains(role, account.Roles);
    }

    [Fact]
    public async Task RemoveRoleFromAccountAsync_Removes_Role_From_Account()
    {
        var account = CreateTestAccount();
        var role = new Role {ID = 1};
        account.Roles = new List<Role> {role};

        var mockUOW = new Mock<IUnitOfWork>();
        var roleService = new RoleService(null!, mockUOW.Object);

        mockUOW.Setup(uow => uow.RoleRepository.FindAsync(role.ID)).ReturnsAsync(role);
        mockUOW.Setup(uow => uow.AccountRepository.FindIncludeRolesAsync(account.ID)).ReturnsAsync(account);

        await roleService.RemoveRoleFromAccountAsync(role.ID, account.ID);

        Assert.DoesNotContain(role, account.Roles);
    }

    [Fact]
    public void CreateRoleAsync_Fails_When_A_Role_With_Same_Name_Exists()
    {
        var existingRole = new Role
        {
            Name = "role"
        };

        var mockUOW = new Mock<IUnitOfWork>();
        var roleService = new RoleService(null!, mockUOW.Object);

        mockUOW.Setup(uow => uow.RoleRepository.FindByNameIgnoreCaseAsync(existingRole.Name)).ReturnsAsync(existingRole);

        Assert.ThrowsAsync<UniqueConstraintException<Role>>(() =>
            roleService.CreateRoleAsync(new RoleWriteDTO {Name = existingRole.Name}));
    }
    
    [Fact]
    public void UpdateRoleAsync_Fails_When_A_Role_With_Same_Name_Exists()
    {
        var updateRole = new Role
        {
            ID = 1,
            Name = "updateRole"
        };
        
        var existingRole = new Role
        {
            Name = "existingRole"
        };

        var mockUOW = new Mock<IUnitOfWork>();
        var roleService = new RoleService(null!, mockUOW.Object);

        mockUOW.Setup(uow => uow.RoleRepository.FindAsync(updateRole.ID)).ReturnsAsync(updateRole);
        mockUOW.Setup(uow => uow.RoleRepository.FindByNameIgnoreCaseAsync(existingRole.Name)).ReturnsAsync(existingRole);

        Assert.ThrowsAsync<UniqueConstraintException<Role>>(() =>
            roleService.UpdateRoleAsync(updateRole.ID, new RoleWriteDTO {Name = existingRole.Name}));
    }

    private Account CreateTestAccount()
    {
        return Account.Create(Guid.NewGuid(), "", "", "", "");
    }
}