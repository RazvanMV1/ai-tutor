using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Features.Subscriptions.Commands.CancelSubscription;
using AiTutor.Application.Features.Subscriptions.Commands.CreateSubscription;
using AiTutor.Application.Features.Subscriptions.Queries.GetAllSubscriptions;
using AiTutor.Application.Features.Subscriptions.Queries.GetSubscriptionByUser;
using AiTutor.Domain.Entities;
using AiTutor.Domain.Enums;
using AiTutor.UnitTests.Application;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AiTutor.UnitTests.Application.Subscriptions;

public class SubscriptionTests : IDisposable
{
    private readonly TestDbContext _context;
    private readonly Mock<IStripeService> _stripeMock;
    private readonly Mock<ILogger<CancelSubscriptionCommandHandler>> _cancelLoggerMock;

    public SubscriptionTests()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new TestDbContext(options);

        _stripeMock = new Mock<IStripeService>();
        _cancelLoggerMock = new Mock<ILogger<CancelSubscriptionCommandHandler>>();
    }

    public void Dispose() => _context.Dispose();

    private CancelSubscriptionCommandHandler CreateCancelHandler() =>
        new(_context, _stripeMock.Object, _cancelLoggerMock.Object);

    private async Task<User> CreateUserAsync()
    {
        var user = User.Create("Test", "User", $"test{Guid.NewGuid():N}@test.com", UserRole.Teacher);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    private async Task<Subscription> CreateSubscriptionAsync(Guid userId,
        SubscriptionType type = SubscriptionType.ParentMonthly)
    {
        var sub = Subscription.Create(userId, type,
            DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(20), 9.99m);
        _context.Subscriptions.Add(sub);
        await _context.SaveChangesAsync();
        return sub;
    }

    // ── CreateSubscription ──────────────────────────────────────────────
    [Fact]
    public async Task CreateSubscription_WithValidData_ShouldSucceed()
    {
        var user = await CreateUserAsync();
        var handler = new CreateSubscriptionCommandHandler(_context);
        var command = new CreateSubscriptionCommand(user.Id, SubscriptionType.ParentMonthly,
            19.99m, DateTime.UtcNow, DateTime.UtcNow.AddMonths(1));

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(201);
        result.Data!.UserId.Should().Be(user.Id);
        result.Data.Type.Should().Be(SubscriptionType.ParentMonthly);
    }

    [Fact]
    public async Task CreateSubscription_WithNonExistentUser_ShouldThrowNotFoundException()
    {
        var handler = new CreateSubscriptionCommandHandler(_context);
        var command = new CreateSubscriptionCommand(Guid.NewGuid(), SubscriptionType.ParentMonthly,
            19.99m, DateTime.UtcNow, DateTime.UtcNow.AddMonths(1));

        var act = async () => await handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    // ── CreateSubscriptionCommandValidator ─────────────────────────────
    [Fact]
    public async Task CreateSubscriptionValidator_WithEmptyUserId_ShouldFail()
    {
        var validator = new CreateSubscriptionCommandValidator();
        var command = new CreateSubscriptionCommand(Guid.Empty, SubscriptionType.ParentMonthly,
            19.99m, DateTime.UtcNow, DateTime.UtcNow.AddMonths(1));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "UserId");
    }

    [Fact]
    public async Task CreateSubscriptionValidator_WithZeroPrice_ShouldFail()
    {
        var validator = new CreateSubscriptionCommandValidator();
        var command = new CreateSubscriptionCommand(Guid.NewGuid(), SubscriptionType.ParentMonthly,
            0m, DateTime.UtcNow, DateTime.UtcNow.AddMonths(1));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Price");
    }

    [Fact]
    public async Task CreateSubscriptionValidator_WithEndDateBeforeStart_ShouldFail()
    {
        var validator = new CreateSubscriptionCommandValidator();
        var command = new CreateSubscriptionCommand(Guid.NewGuid(), SubscriptionType.ParentMonthly,
            19.99m, DateTime.UtcNow.AddMonths(1), DateTime.UtcNow);

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "StartDate");
    }

    // ── CancelSubscription ──────────────────────────────────────────────
    [Fact]
    public async Task CancelSubscription_WithValidData_ShouldSucceed()
    {
        var user = await CreateUserAsync();
        var sub = await CreateSubscriptionAsync(user.Id);
        var handler = CreateCancelHandler();
        var command = new CancelSubscriptionCommand(sub.Id, user.Id);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeTrue();

        // Subscription-ul nu are StripeSubscriptionId (e legacy/seed-uit),
        // deci handler-ul NU trebuie să apeleze Stripe.
        _stripeMock.Verify(
            x => x.CancelSubscriptionAsync(
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task CancelSubscription_WithNonExistentSubscription_ShouldThrowNotFoundException()
    {
        var handler = CreateCancelHandler();
        var command = new CancelSubscriptionCommand(Guid.NewGuid(), Guid.NewGuid());

        var act = async () => await handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CancelSubscription_WithWrongUser_ShouldThrowForbiddenAccessException()
    {
        var user = await CreateUserAsync();
        var sub = await CreateSubscriptionAsync(user.Id);
        var handler = CreateCancelHandler();
        var command = new CancelSubscriptionCommand(sub.Id, Guid.NewGuid());

        var act = async () => await handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ForbiddenAccessException>();
    }

    // ── GetAllSubscriptions ─────────────────────────────────────────────
    [Fact]
    public async Task GetAllSubscriptions_WithData_ShouldReturnPaginatedList()
    {
        var user = await CreateUserAsync();
        await CreateSubscriptionAsync(user.Id);
        await CreateSubscriptionAsync(user.Id, SubscriptionType.ParentYearly);
        var handler = new GetAllSubscriptionsQueryHandler(_context);
        var query = new GetAllSubscriptionsQuery(1, 10);

        var result = await handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.TotalCount.Should().Be(2);
        result.Data.Items.Count.Should().Be(2);
    }

    [Fact]
    public async Task GetAllSubscriptions_WithPagination_ShouldRespectPageSize()
    {
        var user = await CreateUserAsync();
        await CreateSubscriptionAsync(user.Id);
        await CreateSubscriptionAsync(user.Id);
        await CreateSubscriptionAsync(user.Id);
        var handler = new GetAllSubscriptionsQueryHandler(_context);
        var query = new GetAllSubscriptionsQuery(1, 2);

        var result = await handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Items.Count.Should().Be(2);
        result.Data.TotalCount.Should().Be(3);
        result.Data.HasNextPage.Should().BeTrue();
    }

    [Fact]
    public async Task GetAllSubscriptions_WithNoData_ShouldReturnEmpty()
    {
        var handler = new GetAllSubscriptionsQueryHandler(_context);
        var query = new GetAllSubscriptionsQuery(1, 10);

        var result = await handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.TotalCount.Should().Be(0);
    }

    // ── GetSubscriptionByUser ───────────────────────────────────────────
    [Fact]
    public async Task GetSubscriptionByUser_WithSubscriptions_ShouldReturnList()
    {
        var user = await CreateUserAsync();
        await CreateSubscriptionAsync(user.Id);
        await CreateSubscriptionAsync(user.Id, SubscriptionType.School);
        var handler = new GetSubscriptionByUserQueryHandler(_context);
        var query = new GetSubscriptionByUserQuery(user.Id);

        var result = await handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Count.Should().Be(2);
    }

    [Fact]
    public async Task GetSubscriptionByUser_WithNoSubscriptions_ShouldReturnEmptyList()
    {
        var handler = new GetSubscriptionByUserQueryHandler(_context);
        var query = new GetSubscriptionByUserQuery(Guid.NewGuid());

        var result = await handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Count.Should().Be(0);
    }
}
