using System.Security.Claims;
using AiTutor.Infrastructure.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace AiTutor.UnitTests.Infrastructure;

public class CurrentUserServiceTests
{
    private static CurrentUserService BuildService(ClaimsPrincipal? principal)
    {
        var accessor = new Mock<IHttpContextAccessor>();
        if (principal is null)
        {
            accessor.Setup(a => a.HttpContext).Returns((HttpContext?)null);
        }
        else
        {
            var ctx = new DefaultHttpContext { User = principal };
            accessor.Setup(a => a.HttpContext).Returns(ctx);
        }
        return new CurrentUserService(accessor.Object);
    }

    [Fact]
    public void UserId_WhenNoHttpContext_ReturnsNull()
    {
        var sut = BuildService(null);
        sut.UserId.Should().BeNull();
        sut.Email.Should().BeNull();
        sut.IsAuthenticated.Should().BeFalse();
    }

    [Fact]
    public void UserId_WhenAuthenticatedClaimsPresent_ReturnsParsedValues()
    {
        var id = Guid.NewGuid();
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, id.ToString()),
            new Claim(ClaimTypes.Email, "user@test.com")
        }, authenticationType: "TestAuth");
        var principal = new ClaimsPrincipal(identity);

        var sut = BuildService(principal);

        sut.UserId.Should().Be(id);
        sut.Email.Should().Be("user@test.com");
        sut.IsAuthenticated.Should().BeTrue();
    }

    [Fact]
    public void UserId_WhenNoNameIdentifierClaim_ReturnsNull()
    {
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Email, "x@y.com")
        }, "TestAuth");
        var sut = BuildService(new ClaimsPrincipal(identity));

        sut.UserId.Should().BeNull();
        sut.Email.Should().Be("x@y.com");
        sut.IsAuthenticated.Should().BeTrue();
    }
}
