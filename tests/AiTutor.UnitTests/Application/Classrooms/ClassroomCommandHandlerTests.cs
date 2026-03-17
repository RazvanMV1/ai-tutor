using AiTutor.Application.Features.Classrooms.Commands.AddGrade;
using AiTutor.Application.Features.Classrooms.Commands.CreateClassroom;
using AiTutor.Application.Features.Classrooms.Commands.JoinClassroom;
using AiTutor.Domain.Entities;
using AiTutor.Domain.Enums;
using AiTutor.UnitTests.Application;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AiTutor.UnitTests.Application.Classrooms;

public class ClassroomCommandHandlerTests : IDisposable
{
    private readonly TestDbContext _context;

    public ClassroomCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new TestDbContext(options);
    }

    public void Dispose() => _context.Dispose();

    private async Task<User> CreateUserAsync(UserRole role = UserRole.Teacher)
    {
        var email = "test" + Guid.NewGuid().ToString("N") + "@test.com";
        var user = User.Create("Test", "User", email, role);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    private async Task<Subscription> CreateActiveSubscriptionAsync(Guid userId)
    {
        var sub = Subscription.Create(userId, SubscriptionType.ParentMonthly,
            DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(30), 29.99m);
        _context.Subscriptions.Add(sub);
        await _context.SaveChangesAsync();
        return sub;
    }

    private async Task<Classroom> CreateClassroomAsync(Guid teacherId)
    {
        var classroom = Classroom.Create("Test Classroom", "Description",
            SubjectType.Mathematics, teacherId);
        _context.Classrooms.Add(classroom);
        await _context.SaveChangesAsync();
        return classroom;
    }

    // ── CreateClassroom ───────────────────────────────────────────────────────
    [Fact]
    public async Task CreateClassroom_WithValidData_ShouldSucceed()
    {
        var teacher = await CreateUserAsync(UserRole.Teacher);
        await CreateActiveSubscriptionAsync(teacher.Id);
        var handler = new CreateClassroomCommandHandler(_context);
        var command = new CreateClassroomCommand("Matematica 9A", "Descriere", SubjectType.Mathematics, teacher.Id);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Name.Should().Be("Matematica 9A");
        result.Data.ClassCode.Should().StartWith("MAT-");
    }

    [Fact]
    public async Task CreateClassroom_WithoutSubscription_ShouldReturnFailure()
    {
        var teacher = await CreateUserAsync(UserRole.Teacher);
        var handler = new CreateClassroomCommandHandler(_context);
        var command = new CreateClassroomCommand("Matematica 9A", "Descriere", SubjectType.Mathematics, teacher.Id);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(403);
    }

    [Fact]
    public async Task CreateClassroom_WithMaxClassroomsReached_ShouldReturnFailure()
    {
        var teacher = await CreateUserAsync(UserRole.Teacher);
        await CreateActiveSubscriptionAsync(teacher.Id);
        for (int i = 0; i < 5; i++)
            await CreateClassroomAsync(teacher.Id);
        var handler = new CreateClassroomCommandHandler(_context);
        var command = new CreateClassroomCommand("A 6-a clasa", "Descriere", SubjectType.Mathematics, teacher.Id);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(400);
    }

    // ── JoinClassroom ─────────────────────────────────────────────────────────
    [Fact]
    public async Task JoinClassroom_WithValidCode_ShouldSucceed()
    {
        var teacher = await CreateUserAsync(UserRole.Teacher);
        var student = await CreateUserAsync(UserRole.Student);
        var classroom = await CreateClassroomAsync(teacher.Id);
        var handler = new JoinClassroomCommandHandler(_context);
        var command = new JoinClassroomCommand(classroom.ClassCode, student.Id);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        var member = await _context.ClassroomMembers
            .FirstOrDefaultAsync(m => m.ClassroomId == classroom.Id && m.StudentId == student.Id);
        member.Should().NotBeNull();
    }

    [Fact]
    public async Task JoinClassroom_WithInvalidCode_ShouldReturnFailure()
    {
        var student = await CreateUserAsync(UserRole.Student);
        var handler = new JoinClassroomCommandHandler(_context);
        var command = new JoinClassroomCommand("INVALID-CODE", student.Id);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task JoinClassroom_WhenAlreadyMember_ShouldReturnFailure()
    {
        var teacher = await CreateUserAsync(UserRole.Teacher);
        var student = await CreateUserAsync(UserRole.Student);
        var classroom = await CreateClassroomAsync(teacher.Id);
        var member = ClassroomMember.Create(classroom.Id, student.Id);
        _context.ClassroomMembers.Add(member);
        await _context.SaveChangesAsync();
        var handler = new JoinClassroomCommandHandler(_context);
        var command = new JoinClassroomCommand(classroom.ClassCode, student.Id);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(400);
    }

    // ── AddGrade ──────────────────────────────────────────────────────────────
    [Fact]
    public async Task AddGrade_WithValidData_ShouldSucceed()
    {
        var teacher = await CreateUserAsync(UserRole.Teacher);
        var student = await CreateUserAsync(UserRole.Student);
        var classroom = await CreateClassroomAsync(teacher.Id);
        var member = ClassroomMember.Create(classroom.Id, student.Id);
        _context.ClassroomMembers.Add(member);
        await _context.SaveChangesAsync();
        var handler = new AddGradeCommandHandler(_context);
        var command = new AddGradeCommand(classroom.Id, student.Id, teacher.Id, 9, "Raspuns excelent");

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Value.Should().Be(9);
        result.Data.StudentId.Should().Be(student.Id);
    }

    [Fact]
    public async Task AddGrade_WithInvalidValue_ShouldReturnFailureOrThrow()
    {
        var teacher = await CreateUserAsync(UserRole.Teacher);
        var student = await CreateUserAsync(UserRole.Student);
        var classroom = await CreateClassroomAsync(teacher.Id);
        var member = ClassroomMember.Create(classroom.Id, student.Id);
        _context.ClassroomMembers.Add(member);
        await _context.SaveChangesAsync();
        var handler = new AddGradeCommandHandler(_context);
        var command = new AddGradeCommand(classroom.Id, student.Id, teacher.Id, 11, "Nota invalida");

        var act = async () => await handler.Handle(command, CancellationToken.None);
        var result = await handler.Handle(
            new AddGradeCommand(classroom.Id, student.Id, teacher.Id, 11, "test"),
            CancellationToken.None).ContinueWith(t => t.IsFaulted || (t.IsCompletedSuccessfully && !t.Result.IsSuccess));

        result.Should().BeTrue();
    }

    [Fact]
    public async Task AddGrade_WithNonTeacher_ShouldThrowForbiddenAccessException()
    {
        var teacher = await CreateUserAsync(UserRole.Teacher);
        var student = await CreateUserAsync(UserRole.Student);
        var classroom = await CreateClassroomAsync(teacher.Id);
        var fakeTeacher = await CreateUserAsync(UserRole.Teacher);
        var handler = new AddGradeCommandHandler(_context);
        var command = new AddGradeCommand(classroom.Id, student.Id, fakeTeacher.Id, 8, "Test");

        var act = async () => await handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<AiTutor.Application.Common.Exceptions.ForbiddenAccessException>();
    }

}
