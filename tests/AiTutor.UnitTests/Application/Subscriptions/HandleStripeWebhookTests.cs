using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models.Stripe;
using AiTutor.Application.Features.Subscriptions.Commands.HandleStripeWebhook;
using AiTutor.Domain.Entities;
using AiTutor.Domain.Enums;
using AiTutor.UnitTests.Application;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace AiTutor.UnitTests.Application.Subscriptions;

public class HandleStripeWebhookTests
{
    private const string TestPriceId = "price_test_123";
    private const decimal TestPrice = 49.99m;

    private static Subscription NewPending(Guid userId) =>
        Subscription.CreatePending(userId, SubscriptionType.ParentMonthly, TestPrice, TestPriceId);

    private static TestDbContext NewCtx() =>
        new(new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    private static StripeWebhookEvent CheckoutCompletedEvent(
        string eventId, Guid userId, string subId = "sub_123", string customerId = "cus_123") =>
        new(
            Id: eventId,
            Type: "checkout.session.completed",
            Subscription: null,
            CheckoutSession: new StripeCheckoutSessionData(
                SessionId: "cs_test_1",
                CustomerId: customerId,
                SubscriptionId: subId,
                AppUserId: userId,
                AppSubscriptionType: SubscriptionType.ParentMonthly));

    private static StripeWebhookEvent SubscriptionUpdatedEvent(
        string eventId, string status, string subId = "sub_123",
        string customerId = "cus_123", Guid? appUserId = null) =>
        new(
            Id: eventId,
            Type: "customer.subscription.updated",
            Subscription: new StripeSubscriptionData(
                SubscriptionId: subId,
                CustomerId: customerId,
                PriceId: TestPriceId,
                Status: status,
                CurrentPeriodStart: DateTime.UtcNow,
                CurrentPeriodEnd: DateTime.UtcNow.AddMonths(1),
                AppUserId: appUserId,
                AppSubscriptionType: SubscriptionType.ParentMonthly),
            CheckoutSession: null);

    private static StripeWebhookEvent SubscriptionDeletedEvent(
        string eventId, string subId = "sub_123") =>
        new(
            Id: eventId,
            Type: "customer.subscription.deleted",
            Subscription: new StripeSubscriptionData(
                SubscriptionId: subId,
                CustomerId: "cus_123",
                PriceId: TestPriceId,
                Status: "canceled",
                CurrentPeriodStart: DateTime.UtcNow.AddMonths(-1),
                CurrentPeriodEnd: DateTime.UtcNow,
                AppUserId: null,
                AppSubscriptionType: null),
            CheckoutSession: null);

    private static (HandleStripeWebhookCommandHandler handler, TestDbContext ctx, Mock<IStripeService> stripe)
        BuildSut()
    {
        var ctx = NewCtx();
        var stripe = new Mock<IStripeService>();
        var handler = new HandleStripeWebhookCommandHandler(
            ctx, stripe.Object,
            NullLogger<HandleStripeWebhookCommandHandler>.Instance);
        return (handler, ctx, stripe);
    }

    [Fact]
    public async Task Handle_WhenSignatureInvalid_ReturnsFailure()
    {
        var (handler, _, stripe) = BuildSut();
        stripe.Setup(s => s.ConstructWebhookEvent(It.IsAny<string>(), It.IsAny<string>()))
              .Throws(new InvalidOperationException("bad signature"));

        var result = await handler.Handle(
            new HandleStripeWebhookCommand("payload", "sig"), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Invalid webhook signature");
    }

    [Fact]
    public async Task Handle_UnknownEventType_ReturnsSuccess_NoChanges()
    {
        var (handler, ctx, stripe) = BuildSut();
        stripe.Setup(s => s.ConstructWebhookEvent(It.IsAny<string>(), It.IsAny<string>()))
              .Returns(new StripeWebhookEvent("evt_x", "invoice.created", null, null));

        var result = await handler.Handle(
            new HandleStripeWebhookCommand("p", "s"), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        ctx.Subscriptions.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_CheckoutCompleted_LinksPendingSubscription()
    {
        var (handler, ctx, stripe) = BuildSut();
        var userId = Guid.NewGuid();

        ctx.Subscriptions.Add(NewPending(userId));
        await ctx.SaveChangesAsync();

        stripe.Setup(s => s.ConstructWebhookEvent(It.IsAny<string>(), It.IsAny<string>()))
              .Returns(CheckoutCompletedEvent("evt_co_1", userId));

        var result = await handler.Handle(
            new HandleStripeWebhookCommand("p", "s"), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        var saved = ctx.Subscriptions.Single();
        saved.StripeSubscriptionId.Should().Be("sub_123");
        saved.StripeCustomerId.Should().Be("cus_123");
        saved.HasProcessedEvent("evt_co_1").Should().BeTrue();
    }

    [Fact]
    public async Task Handle_CheckoutCompleted_WithoutAppUserId_DoesNothing()
    {
        var (handler, ctx, stripe) = BuildSut();

        stripe.Setup(s => s.ConstructWebhookEvent(It.IsAny<string>(), It.IsAny<string>()))
              .Returns(new StripeWebhookEvent(
                  "evt_co_2", "checkout.session.completed", null,
                  new StripeCheckoutSessionData("cs_1", "cus_1", "sub_1", null, null)));

        var result = await handler.Handle(
            new HandleStripeWebhookCommand("p", "s"), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        ctx.Subscriptions.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_CheckoutCompleted_NoPendingSubscription_DoesNothing()
    {
        var (handler, ctx, stripe) = BuildSut();
        stripe.Setup(s => s.ConstructWebhookEvent(It.IsAny<string>(), It.IsAny<string>()))
              .Returns(CheckoutCompletedEvent("evt_co_3", Guid.NewGuid()));

        var result = await handler.Handle(
            new HandleStripeWebhookCommand("p", "s"), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        ctx.Subscriptions.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_CheckoutCompleted_DuplicateEventId_IsIdempotent()
    {
        var (handler, ctx, stripe) = BuildSut();
        var userId = Guid.NewGuid();
        var pending = NewPending(userId);
        pending.RecordStripeEvent("evt_dup");
        ctx.Subscriptions.Add(pending);
        await ctx.SaveChangesAsync();

        stripe.Setup(s => s.ConstructWebhookEvent(It.IsAny<string>(), It.IsAny<string>()))
              .Returns(CheckoutCompletedEvent("evt_dup", userId));

        var result = await handler.Handle(
            new HandleStripeWebhookCommand("p", "s"), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        ctx.Subscriptions.Single().StripeSubscriptionId.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task Handle_SubscriptionUpdated_Active_MarksActive()
    {
        var (handler, ctx, stripe) = BuildSut();
        var sub = NewPending(Guid.NewGuid());
        sub.LinkStripe("cus_123", "sub_123");
        ctx.Subscriptions.Add(sub);
        await ctx.SaveChangesAsync();

        stripe.Setup(s => s.ConstructWebhookEvent(It.IsAny<string>(), It.IsAny<string>()))
              .Returns(SubscriptionUpdatedEvent("evt_up_1", "active"));

        var result = await handler.Handle(
            new HandleStripeWebhookCommand("p", "s"), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        var saved = ctx.Subscriptions.Single();
        saved.Status.Should().Be(SubscriptionStatus.Active);
        saved.HasProcessedEvent("evt_up_1").Should().BeTrue();
    }

    [Fact]
    public async Task Handle_SubscriptionUpdated_PastDue_MarksPastDue()
    {
        var (handler, ctx, stripe) = BuildSut();
        var sub = NewPending(Guid.NewGuid());
        sub.LinkStripe("cus_123", "sub_123");
        sub.MarkActive(DateTime.UtcNow.AddDays(-30), DateTime.UtcNow.AddDays(1));
        ctx.Subscriptions.Add(sub);
        await ctx.SaveChangesAsync();

        stripe.Setup(s => s.ConstructWebhookEvent(It.IsAny<string>(), It.IsAny<string>()))
              .Returns(SubscriptionUpdatedEvent("evt_up_2", "past_due"));

        var result = await handler.Handle(
            new HandleStripeWebhookCommand("p", "s"), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        ctx.Subscriptions.Single().Status.Should().Be(SubscriptionStatus.PastDue);
    }

    [Fact]
    public async Task Handle_SubscriptionUpdated_NoLocalSubscription_NoOp()
    {
        var (handler, ctx, stripe) = BuildSut();
        stripe.Setup(s => s.ConstructWebhookEvent(It.IsAny<string>(), It.IsAny<string>()))
              .Returns(SubscriptionUpdatedEvent("evt_up_3", "active", subId: "sub_unknown"));

        var result = await handler.Handle(
            new HandleStripeWebhookCommand("p", "s"), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        ctx.Subscriptions.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_SubscriptionDeleted_MarksCanceled()
    {
        var (handler, ctx, stripe) = BuildSut();
        var sub = NewPending(Guid.NewGuid());
        sub.LinkStripe("cus_123", "sub_123");
        sub.MarkActive(DateTime.UtcNow.AddDays(-30), DateTime.UtcNow.AddDays(1));
        ctx.Subscriptions.Add(sub);
        await ctx.SaveChangesAsync();

        stripe.Setup(s => s.ConstructWebhookEvent(It.IsAny<string>(), It.IsAny<string>()))
              .Returns(SubscriptionDeletedEvent("evt_del_1"));

        var result = await handler.Handle(
            new HandleStripeWebhookCommand("p", "s"), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        ctx.Subscriptions.Single().Status.Should().Be(SubscriptionStatus.Canceled);
    }

    [Fact]
    public async Task Handle_SubscriptionUpdated_FallbackToPendingByAppUserId()
    {
        var (handler, ctx, stripe) = BuildSut();
        var userId = Guid.NewGuid();
        ctx.Subscriptions.Add(NewPending(userId));
        await ctx.SaveChangesAsync();

        stripe.Setup(s => s.ConstructWebhookEvent(It.IsAny<string>(), It.IsAny<string>()))
              .Returns(SubscriptionUpdatedEvent(
                  "evt_up_4", "active",
                  subId: "sub_new", appUserId: userId));

        var result = await handler.Handle(
            new HandleStripeWebhookCommand("p", "s"), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        var saved = ctx.Subscriptions.Single();
        saved.StripeSubscriptionId.Should().Be("sub_new");
        saved.Status.Should().Be(SubscriptionStatus.Active);
    }
}
