using System;
using Auth.Core.Entities;
using Xunit;

namespace Auth.Tests.Entities;

public class RefreshTokenTests
{
    [Fact]
    public void IsExpired_Returns_True_When_Expired()
    {
        var token = new RefreshToken
        {
            Expires = DateTime.Now.Subtract(TimeSpan.FromSeconds(1))
        };

        Assert.True(token.IsExpired);
    }
    
    [Fact]
    public void IsExpired_Returns_False_When_Not_Expired()
    {
        var token = new RefreshToken
        {
            Expires = DateTime.Now.Add(TimeSpan.FromSeconds(1))
        };

        Assert.False(token.IsExpired);
    }

    [Fact]
    public void Token_Is_Revoked_When_Replaced()
    {
        var oldToken = new RefreshToken();
        var newToken = new RefreshToken();

        oldToken.ReplaceWith(newToken);

        Assert.True(oldToken.IsRevoked);
    }
}