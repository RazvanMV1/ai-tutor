using AiTutor.Application.Features.Users.Commands.LinkParent;
using AiTutor.Application.Features.Users.Commands.UnlinkParent;
using AiTutor.Application.Features.Users.Queries.GetChildren;
using AiTutor.Application.Features.Users.Queries.GetInvitationCode;
using AiTutor.Application.Features.Users.Queries.GetMyParent;
using AiTutor.Domain.Entities;
using AiTutor.Domain.Enums;
using AiTutor.UnitTests.Application;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AiTutor.UnitTests.Application.Users;

public class ParentChildHandlerTests : IDisposable
{
    private readonly TestDbContext _context;

    public ParentChildHandlerTests()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new TestDbContext(options);
    }

    public void Dispose() => _context.Dispose();

    private async Task<User> CreateUserAsync(UserRole role)
    {
        var user = User.Create("First", "Last", $"u{Guid.NewGuid():N}@t.com", role);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    // ── LinkParent ──────────────────────────────────────────────────

    [Fact]
    public async Task LinkParent_WithValidCode_ShouldSucceed()
    {
        var parent = await CreateUserAsync(UserRole.Parent);
        var student = await CreateUserAsync(UserRole.Student);
        var handler = new LinkParentCommandHandler(_context);

        var result = await handler.Handle(
            new LinkParentCommand(student.Id, parent.InvitationCode!), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.ParentId.Should().Be(parent.Id);
        var refreshed = await _context.Users.FirstAsync(u => u.Id == student.Id);
        refreshed.ParentId.Should().Be(parent.Id);
    }

    [Fact]
    public async Task LinkParent_WithNonExistentStudent_ShouldReturnFailure()
    {
        var handler = new LinkParentCommandHandler(_context);
        var result = await handler.Handle(
            new LinkParentCommand(Guid.NewGuid(), "PAR-XXXXXX"), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task LinkParent_WithNonStudentUser_ShouldReturnFailure()
    {
        var teacher = await CreateUserAsync(UserRole.Teacher);
        var handler = new LinkParentCommandHandler(_context);

        var result = await handler.Handle(
            new LinkParentCommand(teacher.Id, "PAR-XXXXXX"), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task LinkParent_WithEmptyCode_ShouldReturnFailure()
    {
        var student = await CreateUserAsync(UserRole.Student);
        var handler = new LinkParentCommandHandler(_context);

        var result = await handler.Handle(
            new LinkParentCommand(student.Id, "   "), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task LinkParent_WithInvalidCode_ShouldReturnFailure()
    {
        var student = await CreateUserAsync(UserRole.Student);
        var handler = new LinkParentCommandHandler(_context);

        var result = await handler.Handle(
            new LinkParentCommand(student.Id, "PAR-INVALID"), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(404);
    }

    // ── UnlinkParent ────────────────────────────────────────────────

    [Fact]
    public async Task UnlinkParent_WithLinkedStudent_ShouldSucceed()
    {
        var parent = await CreateUserAsync(UserRole.Parent);
        var student = await CreateUserAsync(UserRole.Student);
        student.LinkToParent(parent.Id);
        await _context.SaveChangesAsync();

        var handler = new UnlinkParentCommandHandler(_context);
        var result = await handler.Handle(
            new UnlinkParentCommand(student.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        var refreshed = await _context.Users.FirstAsync(u => u.Id == student.Id);
        refreshed.ParentId.Should().BeNull();
    }

    [Fact]
    public async Task UnlinkParent_WithNonExistentStudent_ShouldReturnFailure()
    {
        var handler = new UnlinkParentCommandHandler(_context);
        var result = await handler.Handle(
            new UnlinkParentCommand(Guid.NewGuid()), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task UnlinkParent_WithNonStudent_ShouldReturnFailure()
    {
        var teacher = await CreateUserAsync(UserRole.Teacher);
        var handler = new UnlinkParentCommandHandler(_context);

        var result = await handler.Handle(
            new UnlinkParentCommand(teacher.Id), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task UnlinkParent_NotLinked_ShouldReturnFailure()
    {
        var student = await CreateUserAsync(UserRole.Student);
        var handler = new UnlinkParentCommandHandler(_context);

        var result = await handler.Handle(
            new UnlinkParentCommand(student.Id), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(400);
    }

    // ── GetChildren ─────────────────────────────────────────────────

    [Fact]
    public async Task GetChildren_WithMultipleChildren_ShouldReturnSortedList()
    {
        var parent = await CreateUserAsync(UserRole.Parent);
        var c1 = User.Create("Bob", "Last", $"b{Guid.NewGuid():N}@t.com", UserRole.Student);
        var c2 = User.Create("Alice", "Last", $"a{Guid.NewGuid():N}@t.com", UserRole.Student);
        c1.LinkToParent(parent.Id);
        c2.LinkToParent(parent.Id);
        _context.Users.AddRange(c1, c2);
        await _context.SaveChangesAsync();

        var handler = new GetChildrenQueryHandler(_context);
        var result = await handler.Handle(
            new GetChildrenQuery(parent.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Should().HaveCount(2);
        result.Data[0].FullName.Should().StartWith("Alice"); // sorted by FirstName
    }

    [Fact]
    public async Task GetChildren_WithNoChildren_ShouldReturnEmpty()
    {
        var parent = await CreateUserAsync(UserRole.Parent);
        var handler = new GetChildrenQueryHandler(_context);

        var result = await handler.Handle(
            new GetChildrenQuery(parent.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Should().BeEmpty();
    }

    // ── GetMyParent ─────────────────────────────────────────────────

    [Fact]
    public async Task GetMyParent_WithLinkedParent_ShouldReturnParent()
    {
        var parent = await CreateUserAsync(UserRole.Parent);
        var student = await CreateUserAsync(UserRole.Student);
        student.LinkToParent(parent.Id);
        await _context.SaveChangesAsync();

        var handler = new GetMyParentQueryHandler(_context);
        var result = await handler.Handle(
            new GetMyParentQuery(student.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.ParentId.Should().Be(parent.Id);
    }

    [Fact]
    public async Task GetMyParent_NotLinked_ShouldReturnNullData()
    {
        var student = await CreateUserAsync(UserRole.Student);
        var handler = new GetMyParentQueryHandler(_context);

        var result = await handler.Handle(
            new GetMyParentQuery(student.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeNull();
    }

    [Fact]
    public async Task GetMyParent_WithNonExistentStudent_ShouldReturnFailure()
    {
        var handler = new GetMyParentQueryHandler(_context);
        var result = await handler.Handle(
            new GetMyParentQuery(Guid.NewGuid()), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(404);
    }

    // ── GetInvitationCode ───────────────────────────────────────────

    [Fact]
    public async Task GetInvitationCode_AsParent_ShouldReturnCode()
    {
        var parent = await CreateUserAsync(UserRole.Parent);
        var handler = new GetInvitationCodeQueryHandler(_context);

        var result = await handler.Handle(
            new GetInvitationCodeQuery(parent.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.InvitationCode.Should().StartWith("PAR-");
    }

    [Fact]
    public async Task GetInvitationCode_NonExistentUser_ShouldReturnFailure()
    {
        var handler = new GetInvitationCodeQueryHandler(_context);
        var result = await handler.Handle(
            new GetInvitationCodeQuery(Guid.NewGuid()), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task GetInvitationCode_NonParent_ShouldReturnFailure()
    {
        var teacher = await CreateUserAsync(UserRole.Teacher);
        var handler = new GetInvitationCodeQueryHandler(_context);

        var result = await handler.Handle(
            new GetInvitationCodeQuery(teacher.Id), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(400);
    }
}
