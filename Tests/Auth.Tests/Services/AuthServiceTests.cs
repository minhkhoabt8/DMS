using System;
using System.Threading.Tasks;
using Auth.Core.Entities;
using Auth.Core.Exceptions;
using Auth.Infrastructure.DTOs.Authentication;
using Auth.Infrastructure.Services.Implementations;
using Auth.Infrastructure.Services.Interfaces;
using Auth.Infrastructure.UOW;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace Auth.Tests.Services;

public class AuthServiceTests
{
    [Fact]
    public async Task Login_Throws_UnauthorizedException_When_Account_Does_Not_Have_Access_To_DMS_Module()
    {
        var authOutput = new SMAuthDTO()
        {
            SystemModules = Array.Empty<SMModuleDTO>()
        };
        var mockSMService = new Mock<ISystemManagementService>();
        mockSMService.Setup(sm => sm.LoginAsync(It.IsAny<LoginInputDTO>())).ReturnsAsync(authOutput);
        var authService = new AuthService(mockSMService.Object, null!, null!, null!
            , null!);

        await Assert.ThrowsAsync<UnauthorizedException>(() =>
            authService.LoginWithUsernamePasswordAsync(new LoginInputDTO()));
    }

    [Fact]
    public async Task Login_Creates_New_Account_When_User_Not_Exists_In_DMS_System()
    {
        var accID = Guid.NewGuid();

        var authOutput = new SMAuthDTO()
        {
            UserID = accID,
            SystemModules = new[] {new SMModuleDTO {Name = "Library"}},
            Access_Token =
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySWQiOiIyMGEzMmFlNC01YTU4LTQ2OTAtYjU0NC1kYzg5OGExZGIyNGQiLCJFbWFpbCI6ImxpYnJhcnlAZXhhbXBsZS5jb20iLCJGdWxsTmFtZSI6InN0cmluZyIsIlVzZXJOYW1lIjoibGlicmFyeSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IlRyYWluZWUiLCJNb2R1bGVzIjpbIkJCQiIsIkRFTU8gVEVTVDIiLCJBU1NFVCIsIkxJQlJBUlkiLCJMTVMiLCJMSUVVIiwiVEVTVE5FV05FVyJdLCJQaG9uZU51bWJlciI6InN0cmluZyIsImV4cCI6MTY0NDAyNTQ3MCwiaXNzIjoiVmlldEpldF9BcGlHYXRld2F5IiwiYXVkIjoiVmlldEpldF9BcGlHYXRld2F5In0.E-FiF_rAJAXWAtOLzJR5KPoAt3Yps7kYxLSortIW9dQ"
        };

        var mockSMService = new Mock<ISystemManagementService>();
        var mockUOW = new Mock<IUnitOfWork>();
        var mockTokenService = new Mock<ITokenService>();

        mockSMService.Setup(sm => sm.LoginAsync(It.IsAny<LoginInputDTO>())).ReturnsAsync(authOutput);
        mockTokenService.Setup(ts => ts.GenerateRefreshToken(It.IsAny<Account>())).Returns(new RefreshToken());
        mockUOW.Setup(m => m.AccountRepository.FindAsync(accID)).ReturnsAsync((Account?) null);
        mockUOW.Setup(m => m.RefreshTokenRepository.AddAsync(It.IsAny<RefreshToken>()));
        mockUOW.Setup(m => m.CommitAsync()).ReturnsAsync(1);

        var authService = new AuthService(mockSMService.Object,
            new Mock<IRoleService>().Object,
            mockUOW.Object,
            mockTokenService.Object,
            new Mock<IConfiguration>().Object);

        await authService.LoginWithUsernamePasswordAsync(new LoginInputDTO());

        mockUOW.Verify(m => m.AccountRepository.AddAsync(It.Is<Account>(acc => acc.ID == accID)));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task Login_With_Token_Fails_When_Token_Is_Empty_Or_Null(string? token)
    {
        var authService = new AuthService(null!, null!, null!, null!, null!);

        await Assert.ThrowsAsync<InvalidRefreshTokenException>(() => authService.LoginWithRefreshTokenAsync(token));
    }

    [Fact]
    public async Task Login_With_Token_Revoke_Token_Line_When_A_Revoked_Token_Is_Used()
    {
        var replacementToken = new RefreshToken
        {
            Token = "replace",
            IsRevoked = false
        };

        var token = new RefreshToken
        {
            Token = "original",
            IsRevoked = true,
            ReplacedByToken = replacementToken.Token
        };

        var mockUOW = new Mock<IUnitOfWork>();
        var authService = new AuthService(null!, null!, mockUOW.Object, null!, null!);

        mockUOW.Setup(uow => uow.RefreshTokenRepository.FindByTokenIncludeAccountAsync(token.Token))
            .ReturnsAsync(token);
        mockUOW.Setup(uow => uow.RefreshTokenRepository.FindByTokenAsync(replacementToken.Token))
            .ReturnsAsync(replacementToken);


        await Assert.ThrowsAsync<InvalidRefreshTokenException>(
            () => authService.LoginWithRefreshTokenAsync(token.Token));
        Assert.True(replacementToken.IsRevoked);
    }

    [Fact]
    public void Login_With_Token_Fails_When_Token_Is_Expired()
    {
        var token = new RefreshToken
        {
            Token = "token",
            Expires = DateTime.Now.Subtract(TimeSpan.FromSeconds(1))
        };
        
        var mockUOW = new Mock<IUnitOfWork>();
        var authService = new AuthService(null!, null!, mockUOW.Object, null!, null!);

        mockUOW.Setup(uow => uow.RefreshTokenRepository.FindByTokenIncludeAccountAsync(token.Token))
            .ReturnsAsync(token);

        Assert.ThrowsAsync<InvalidRefreshTokenException>(() => authService.LoginWithRefreshTokenAsync(token.Token));
    }
}