using AiTutor.Application.Common.Models.Stripe;
using AiTutor.Domain.Entities;
using AiTutor.Domain.Enums;
using AiTutor.Domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace AiTutor.UnitTests.Domain;

public class SubscriptionEntityTests
{
    [Fact]
    public void Create_WithValidData_ShouldSucceed()
    {
        var s = Subscription.Create(Guid.NewGuid(), SubscriptionType.ParentMonthly,
            DateTime.UtcNow, DateTime.UtcNow.AddDays(30), 49.99m);

        s.IsActive.Should().BeTrue();
        s.Status.Should().Be(SubscriptionStatus.Active);
        s.Price.Should().Be(49.99m);
    }

    [Fact]
    public void Create_WithEndBeforeStart_ShouldThrow()
        => FluentActions.Invoking(() => Subscription.Create(
                Guid.NewGuid(), SubscriptionType.ParentMonthly,
                DateTime.UtcNow, DateTime.UtcNow.AddDays(-1), 10m))
            .Should().Throw<DomainException>();

    [Fact]
    public void Create_WithNegativePrice_ShouldThrow()
        => FluentActions.Invoking(() => Subscription.Create(
                Guid.NewGuid(), SubscriptionType.ParentMonthly,
                DateTime.UtcNow, DateTime.UtcNow.AddDays(30), -1m))
            .Should().Throw<DomainException>();

    [Fact]
    public void CreatePending_WithValidData_ShouldSucceed()
    {
        var s = Subscription.CreatePending(Guid.NewGuid(),
            SubscriptionType.ParentMonthly, 49.99m, "price_123", "cus_456");

        s.Status.Should().Be(SubscriptionStatus.Pending);
        s.IsActive.Should().BeFalse();
        s.StripePriceId.Should().Be("price_123");
        s.StripeCustomerId.Should().Be("cus_456");
    }

    [Fact]
    public void CreatePending_WithoutPriceId_ShouldThrow()
        => FluentActions.Invoking(() => Subscription.CreatePending(
                Guid.NewGuid(), SubscriptionType.ParentMonthly, 10m, ""))
            .Should().Throw<DomainException>();

    [Fact]
    public void CreatePending_WithNegativePrice_ShouldThrow()
        => FluentActions.Invoking(() => Subscription.CreatePending(
                Guid.NewGuid(), SubscriptionType.ParentMonthly, -1m, "price_x"))
            .Should().Throw<DomainException>();

    [Fact]
    public void IsValid_ActiveAndNotExpired_ShouldReturnTrue()
    {
        var s = Subscription.Create(Guid.NewGuid(), SubscriptionType.ParentMonthly,
            DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(30), 10m);
        s.IsValid().Should().BeTrue();
    }

    [Fact]
    public void IsValid_Deactivated_ShouldReturnFalse()
    {
        var s = Subscription.Create(Guid.NewGuid(), SubscriptionType.ParentMonthly,
            DateTime.UtcNow, DateTime.UtcNow.AddDays(30), 10m);
        s.Deactivate();
        s.IsValid().Should().BeFalse();
        s.Status.Should().Be(SubscriptionStatus.Canceled);
    }

    [Fact]
    public void Extend_WithValidDate_ShouldUpdate()
    {
        var s = Subscription.Create(Guid.NewGuid(), SubscriptionType.ParentMonthly,
            DateTime.UtcNow, DateTime.UtcNow.AddDays(30), 10m);
        var newEnd = DateTime.UtcNow.AddDays(60);
        s.Extend(newEnd);
        s.EndDate.Should().BeCloseTo(newEnd, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Extend_WithEarlierDate_ShouldThrow()
    {
        var s = Subscription.Create(Guid.NewGuid(), SubscriptionType.ParentMonthly,
            DateTime.UtcNow, DateTime.UtcNow.AddDays(30), 10m);
        FluentActions.Invoking(() => s.Extend(DateTime.UtcNow.AddDays(1)))
            .Should().Throw<DomainException>();
    }

    [Fact]
    public void LinkStripe_WithValidIds_ShouldSetThem()
    {
        var s = Subscription.CreatePending(Guid.NewGuid(),
            SubscriptionType.ParentMonthly, 10m, "price_x");
        s.LinkStripe("cus_1", "sub_1");
        s.StripeCustomerId.Should().Be("cus_1");
        s.StripeSubscriptionId.Should().Be("sub_1");
    }

    [Fact]
    public void LinkStripe_WithEmptyIds_ShouldThrow()
    {
        var s = Subscription.CreatePending(Guid.NewGuid(),
            SubscriptionType.ParentMonthly, 10m, "price_x");
        FluentActions.Invoking(() => s.LinkStripe("", "sub_1")).Should().Throw<DomainException>();
        FluentActions.Invoking(() => s.LinkStripe("cus_1", "")).Should().Throw<DomainException>();
    }

    [Fact]
    public void MarkActive_WithValidPeriod_ShouldUpdate()
    {
        var s = Subscription.CreatePending(Guid.NewGuid(),
            SubscriptionType.ParentMonthly, 10m, "price_x");
        var start = DateTime.UtcNow;
        var end = start.AddDays(30);
        s.MarkActive(start, end);
        s.IsActive.Should().BeTrue();
        s.Status.Should().Be(SubscriptionStatus.Active);
    }

    [Fact]
    public void MarkActive_WithInvalidPeriod_ShouldThrow()
    {
        var s = Subscription.CreatePending(Guid.NewGuid(),
            SubscriptionType.ParentMonthly, 10m, "price_x");
        FluentActions.Invoking(() => s.MarkActive(DateTime.UtcNow, DateTime.UtcNow.AddDays(-1)))
            .Should().Throw<DomainException>();
    }

    [Fact]
    public void MarkPastDue_ShouldSetStatus()
    {
        var s = Subscription.Create(Guid.NewGuid(), SubscriptionType.ParentMonthly,
            DateTime.UtcNow, DateTime.UtcNow.AddDays(30), 10m);
        s.MarkPastDue();
        s.Status.Should().Be(SubscriptionStatus.PastDue);
        s.IsActive.Should().BeTrue(); // grace period
    }

    [Fact]
    public void MarkIncomplete_ShouldDeactivate()
    {
        var s = Subscription.Create(Guid.NewGuid(), SubscriptionType.ParentMonthly,
            DateTime.UtcNow, DateTime.UtcNow.AddDays(30), 10m);
        s.MarkIncomplete();
        s.Status.Should().Be(SubscriptionStatus.Incomplete);
        s.IsActive.Should().BeFalse();
    }

    [Fact]
    public void MarkCanceledByStripe_ShouldDeactivate()
    {
        var s = Subscription.Create(Guid.NewGuid(), SubscriptionType.ParentMonthly,
            DateTime.UtcNow, DateTime.UtcNow.AddDays(30), 10m);
        s.MarkCanceledByStripe();
        s.IsActive.Should().BeFalse();
        s.Status.Should().Be(SubscriptionStatus.Canceled);
    }

    [Fact]
    public void HasProcessedEvent_NoEventStored_ShouldReturnFalse()
    {
        var s = Subscription.Create(Guid.NewGuid(), SubscriptionType.ParentMonthly,
            DateTime.UtcNow, DateTime.UtcNow.AddDays(30), 10m);
        s.HasProcessedEvent("evt_1").Should().BeFalse();
    }

    [Fact]
    public void RecordStripeEvent_ThenHasProcessedEvent_ShouldReturnTrue()
    {
        var s = Subscription.Create(Guid.NewGuid(), SubscriptionType.ParentMonthly,
            DateTime.UtcNow, DateTime.UtcNow.AddDays(30), 10m);
        s.RecordStripeEvent("evt_1");
        s.HasProcessedEvent("evt_1").Should().BeTrue();
        s.HasProcessedEvent("evt_2").Should().BeFalse();
    }

    // ── StripeStatusMapper ──────────────────────────────────────────

    [Theory]
    [InlineData("active", SubscriptionStatus.Active)]
    [InlineData("trialing", SubscriptionStatus.Active)]
    [InlineData("past_due", SubscriptionStatus.PastDue)]
    [InlineData("canceled", SubscriptionStatus.Canceled)]
    [InlineData("unpaid", SubscriptionStatus.Canceled)]
    [InlineData("incomplete", SubscriptionStatus.Incomplete)]
    [InlineData("incomplete_expired", SubscriptionStatus.Incomplete)]
    [InlineData("unknown_status", SubscriptionStatus.Pending)]
    [InlineData(null, SubscriptionStatus.Pending)]
    public void StripeStatusMapper_ShouldMapCorrectly(string? stripeStatus, SubscriptionStatus expected)
    {
        StripeStatusMapper.ToDomainStatus(stripeStatus!).Should().Be(expected);
    }
}
