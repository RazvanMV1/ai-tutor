using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models.Stripe;
using AiTutor.Application.Features.Subscriptions.Commands.CreateCheckoutSession;
using AiTutor.Domain.Entities;
using AiTutor.Domain.Enums;
using AiTutor.UnitTests.Application;
using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace AiTutor.UnitTests.Application.Subscriptions;

public class CreateCheckoutSessionTests : IDisposable
{
    private readonly TestDbContext _context;
    private readonly Mock<IStripeService> _stripeMock;

    public CreateCheckoutSessionTests()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new TestDbContext(options);
        _stripeMock = new Mock<IStripeService>();
    }

    public void Dispose() => _context.Dispose();

    private async Task<User> CreateUserAsync()
    {
        var u = User.Create("First", "Last", $"u{Guid.NewGuid():N}@t.com", UserRole.Parent);
        _context.Users.Add(u);
        await _context.SaveChangesAsync();
        return u;
    }

    private void SetupStripeOk()
    {
        _stripeMock.Setup(s => s.CreateCheckoutSessionAsync(
                It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<SubscriptionType>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CheckoutSessionResult(
                SessionId: "sess_test_123",
                CheckoutUrl: "https://stripe.test/checkout",
                StripeCustomerId: "cus_test",
                StripePriceId: "price_test"));
    }

    // ── Handler ─────────────────────────────────────────────────────

    [Fact]
    public async Task Handle_WithValidUser_ShouldCreatePendingSubscription()
    {
        var user = await CreateUserAsync();
        SetupStripeOk();
        var handler = new CreateCheckoutSessionCommandHandler(_context, _stripeMock.Object);

        var result = await handler.Handle(
            new CreateCheckoutSessionCommand(user.Id, SubscriptionType.ParentMonthly),
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(201);
        result.Data!.SessionId.Should().Be("sess_test_123");
        result.Data.CheckoutUrl.Should().Be("https://stripe.test/checkout");

        var sub = await _context.Subscriptions.FirstOrDefaultAsync();
        sub.Should().NotBeNull();
        sub!.Status.Should().Be(SubscriptionStatus.Pending);
        sub.StripePriceId.Should().Be("price_test");
    }

    [Fact]
    public async Task Handle_WithNonExistentUser_ShouldThrowNotFound()
    {
        var handler = new CreateCheckoutSessionCommandHandler(_context, _stripeMock.Object);
        var act = async () => await handler.Handle(
            new CreateCheckoutSessionCommand(Guid.NewGuid(), SubscriptionType.ParentMonthly),
            CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_WhenAlreadyActiveSubscription_ShouldReturnConflict()
    {
        var user = await CreateUserAsync();
        var existing = Subscription.Create(user.Id, SubscriptionType.ParentMonthly,
            DateTime.UtcNow, DateTime.UtcNow.AddDays(30), 49.99m);
        _context.Subscriptions.Add(existing);
        await _context.SaveChangesAsync();
        SetupStripeOk();

        var handler = new CreateCheckoutSessionCommandHandler(_context, _stripeMock.Object);
        var result = await handler.Handle(
            new CreateCheckoutSessionCommand(user.Id, SubscriptionType.ParentMonthly),
            CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(409);
    }

    [Fact]
    public async Task Handle_WhenAlreadyPendingSubscription_ShouldReturnConflict()
    {
        var user = await CreateUserAsync();
        var pending = Subscription.CreatePending(user.Id, SubscriptionType.ParentMonthly,
            49.99m, "price_x");
        _context.Subscriptions.Add(pending);
        await _context.SaveChangesAsync();
        SetupStripeOk();

        var handler = new CreateCheckoutSessionCommandHandler(_context, _stripeMock.Object);
        var result = await handler.Handle(
            new CreateCheckoutSessionCommand(user.Id, SubscriptionType.ParentMonthly),
            CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(409);
    }

    [Theory]
    [InlineData(SubscriptionType.ParentYearly)]
    [InlineData(SubscriptionType.School)]
    public async Task Handle_DifferentPlans_ShouldUseCorrectPrice(SubscriptionType type)
    {
        var user = await CreateUserAsync();
        SetupStripeOk();
        var handler = new CreateCheckoutSessionCommandHandler(_context, _stripeMock.Object);

        var result = await handler.Handle(
            new CreateCheckoutSessionCommand(user.Id, type), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        var sub = await _context.Subscriptions.FirstAsync();
        sub.Price.Should().BeGreaterThan(0);
    }

    // ── Validator ───────────────────────────────────────────────────

    [Fact]
    public async Task Validator_WithValidCommand_ShouldPass()
    {
        var v = new CreateCheckoutSessionCommandValidator();
        var cmd = new CreateCheckoutSessionCommand(Guid.NewGuid(), SubscriptionType.ParentMonthly);

        var result = await v.TestValidateAsync(cmd);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Validator_WithEmptyUserId_ShouldFail()
    {
        var v = new CreateCheckoutSessionCommandValidator();
        var cmd = new CreateCheckoutSessionCommand(Guid.Empty, SubscriptionType.ParentMonthly);

        var result = await v.TestValidateAsync(cmd);

        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public async Task Validator_WithFreePlan_ShouldFail()
    {
        var v = new CreateCheckoutSessionCommandValidator();
        var cmd = new CreateCheckoutSessionCommand(Guid.NewGuid(), SubscriptionType.Free);

        var result = await v.TestValidateAsync(cmd);

        result.ShouldHaveValidationErrorFor(x => x.SubscriptionType);
    }

    [Fact]
    public async Task Validator_WithInvalidEnum_ShouldFail()
    {
        var v = new CreateCheckoutSessionCommandValidator();
        var cmd = new CreateCheckoutSessionCommand(Guid.NewGuid(), (SubscriptionType)999);

        var result = await v.TestValidateAsync(cmd);

        result.ShouldHaveValidationErrorFor(x => x.SubscriptionType);
    }
}
