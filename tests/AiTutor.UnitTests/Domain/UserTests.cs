using AiTutor.Domain.Entities;
using AiTutor.Domain.Enums;
using AiTutor.Domain.Events;
using AiTutor.Domain.Exceptions;
using AiTutor.Domain.ValueObjects;
using FluentAssertions;

namespace AiTutor.UnitTests.Domain;

public class UserTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateUser()
    {
        // Arrange & Act
        var user = User.Create("John", "Doe", "john@example.com", UserRole.Student);

        // Assert
        user.Should().NotBeNull();
        user.FirstName.Should().Be("John");
        user.LastName.Should().Be("Doe");
        user.Email.Value.Should().Be("john@example.com");
        user.Role.Should().Be(UserRole.Student);
        user.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Create_ShouldRaiseUserCreatedEvent()
    {
        // Arrange & Act
        var user = User.Create("John", "Doe", "john@example.com", UserRole.Student);

        // Assert
        user.DomainEvents.Should().ContainSingle();
        user.DomainEvents.First().Should().BeOfType<UserCreatedEvent>();
    }

    [Fact]
    public void UpdateProfile_ShouldUpdateNameAndSetUpdatedAt()
    {
        // Arrange
        var user = User.Create("John", "Doe", "john@example.com", UserRole.Student);

        // Act
        user.UpdateProfile("Jane", "Smith");

        // Assert
        user.FirstName.Should().Be("Jane");
        user.LastName.Should().Be("Smith");
        user.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Deactivate_ShouldSetIsActiveFalse()
    {
        // Arrange
        var user = User.Create("John", "Doe", "john@example.com", UserRole.Student);

        // Act
        user.Deactivate();

        // Assert
        user.IsActive.Should().BeFalse();
    }

    [Fact]
    public void FullName_ShouldReturnCombinedName()
    {
        // Arrange
        var user = User.Create("John", "Doe", "john@example.com", UserRole.Student);

        // Assert
        user.FullName.Should().Be("John Doe");
    }
}
