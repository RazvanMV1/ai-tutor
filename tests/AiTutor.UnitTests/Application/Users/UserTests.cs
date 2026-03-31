using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Features.Users.Queries.GetAllUsers;
using AiTutor.Application.Features.Users.Queries.GetUserById;
using AiTutor.Domain.Entities;
using AiTutor.Domain.Enums;
using AiTutor.UnitTests.Application;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AiTutor.UnitTests.Application.Users;

public class UserTests : IDisposable
{
    private readonly TestDbContext _context;

    public UserTests()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new TestDbContext(options);
    }

    public void Dispose() => _context.Dispose();

    private async Task<User> CreateUserAsync(string first = "Test", string last = "User",
        UserRole role = UserRole.Student)
    {
        var user = User.Create(first, last, $"test{Guid.NewGuid():N}@test.com", role);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    // ── GetUserById ────────────────────────────────────────────────────
    [Fact]
    public async Task GetUserById_WithValidId_ShouldReturnUser()
    {
        var user = await CreateUserAsync("John", "Doe");
        var handler = new GetUserByIdQueryHandler(_context);
        var query = new GetUserByIdQuery(user.Id);

        var result = await handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.FullName.Should().Be("John Doe");
        result.Data.Role.Should().Be(UserRole.Student);
    }

    [Fact]
    public async Task GetUserById_WithInvalidId_ShouldThrowNotFoundException()
    {
        var handler = new GetUserByIdQueryHandler(_context);
        var query = new GetUserByIdQuery(Guid.NewGuid());

        var act = async () => await handler.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    // ── GetAllUsers ────────────────────────────────────────────────────
    [Fact]
    public async Task GetAllUsers_WithUsers_ShouldReturnPaginatedList()
    {
        await CreateUserAsync("Alice", "One");
        await CreateUserAsync("Bob", "Two");
        await CreateUserAsync("Charlie", "Three");
        var handler = new GetAllUsersQueryHandler(_context);
        var query = new GetAllUsersQuery(1, 10);

        var result = await handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.TotalCount.Should().Be(3);
        result.Data.Items.Count.Should().Be(3);
    }

    [Fact]
    public async Task GetAllUsers_WithPagination_ShouldRespectPageSize()
    {
        await CreateUserAsync("Alice", "One");
        await CreateUserAsync("Bob", "Two");
        await CreateUserAsync("Charlie", "Three");
        var handler = new GetAllUsersQueryHandler(_context);
        var query = new GetAllUsersQuery(1, 2);

        var result = await handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.TotalCount.Should().Be(3);
        result.Data.Items.Count.Should().Be(2);
        result.Data.TotalPages.Should().Be(2);
        result.Data.HasNextPage.Should().BeTrue();
        result.Data.HasPreviousPage.Should().BeFalse();
    }

    [Fact]
    public async Task GetAllUsers_Page2_ShouldReturnRemainingItems()
    {
        await CreateUserAsync("Alice", "One");
        await CreateUserAsync("Bob", "Two");
        await CreateUserAsync("Charlie", "Three");
        var handler = new GetAllUsersQueryHandler(_context);
        var query = new GetAllUsersQuery(2, 2);

        var result = await handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Items.Count.Should().Be(1);
        result.Data.HasPreviousPage.Should().BeTrue();
        result.Data.HasNextPage.Should().BeFalse();
    }

    [Fact]
    public async Task GetAllUsers_WithNoUsers_ShouldReturnEmpty()
    {
        var handler = new GetAllUsersQueryHandler(_context);
        var query = new GetAllUsersQuery(1, 10);

        var result = await handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.TotalCount.Should().Be(0);
        result.Data.Items.Count.Should().Be(0);
    }
}
