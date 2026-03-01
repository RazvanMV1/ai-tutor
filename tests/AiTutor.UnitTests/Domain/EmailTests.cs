using AiTutor.Domain.ValueObjects;
using FluentAssertions;

namespace AiTutor.UnitTests.Domain;

public class EmailTests
{
    [Theory]
    [InlineData("test@example.com")]
    [InlineData("user.name@domain.org")]
    [InlineData("TEST@EXAMPLE.COM")]
    public void Create_WithValidEmail_ShouldCreateEmail(string email)
    {
        // Act
        var result = Email.Create(email);

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().Be(email.ToLowerInvariant().Trim());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_WithEmptyEmail_ShouldThrowArgumentException(string email)
    {
        // Act
        var act = () => Email.Create(email);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_WithNullEmail_ShouldThrowArgumentException()
    {
        // Act
        var act = () => Email.Create(null!);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_WithoutAtSign_ShouldThrowArgumentException()
    {
        // Act
        var act = () => Email.Create("invalidemail.com");

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Equals_WithSameEmail_ShouldBeTrue()
    {
        // Arrange
        var email1 = Email.Create("test@example.com");
        var email2 = Email.Create("test@example.com");

        // Assert
        email1.Should().Be(email2);
    }
}
