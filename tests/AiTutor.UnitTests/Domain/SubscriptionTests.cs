using AiTutor.Domain.Entities;
using AiTutor.Domain.Enums;
using AiTutor.Domain.Exceptions;
using FluentAssertions;

namespace AiTutor.UnitTests.Domain;

public class SubscriptionTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateSubscription()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var start = DateTime.UtcNow;
        var end = start.AddMonths(1);

        // Act
        var subscription = Subscription.Create(
            userId, SubscriptionType.ParentMonthly, start, end, 29.99m);

        // Assert
        subscription.UserId.Should().Be(userId);
        subscription.Type.Should().Be(SubscriptionType.ParentMonthly);
        subscription.IsActive.Should().BeTrue();
        subscription.Price.Should().Be(29.99m);
    }

    [Fact]
    public void Create_WithEndDateBeforeStartDate_ShouldThrowDomainException()
    {
        // Arrange
        var start = DateTime.UtcNow;
        var end = start.AddDays(-1);

        // Act
        var act = () => Subscription.Create(
            Guid.NewGuid(), SubscriptionType.ParentMonthly, start, end, 29.99m);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("End date must be after start date.");
    }

    [Fact]
    public void Create_WithNegativePrice_ShouldThrowDomainException()
    {
        // Act
        var act = () => Subscription.Create(
            Guid.NewGuid(), SubscriptionType.ParentMonthly,
            DateTime.UtcNow, DateTime.UtcNow.AddMonths(1), -10m);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Price cannot be negative.");
    }

    [Fact]
    public void IsValid_WithActiveAndFutureEndDate_ShouldReturnTrue()
    {
        // Arrange
        var subscription = Subscription.Create(
            Guid.NewGuid(), SubscriptionType.ParentMonthly,
            DateTime.UtcNow, DateTime.UtcNow.AddMonths(1), 29.99m);

        // Assert
        subscription.IsValid().Should().BeTrue();
    }

    [Fact]
    public void Deactivate_ShouldSetIsActiveFalse()
    {
        // Arrange
        var subscription = Subscription.Create(
            Guid.NewGuid(), SubscriptionType.ParentMonthly,
            DateTime.UtcNow, DateTime.UtcNow.AddMonths(1), 29.99m);

        // Act
        subscription.Deactivate();

        // Assert
        subscription.IsActive.Should().BeFalse();
    }
}
