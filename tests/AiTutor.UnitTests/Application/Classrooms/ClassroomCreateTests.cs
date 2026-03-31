using AiTutor.Application.Features.Classrooms.Commands.CreateClassroom;
using AiTutor.Application.Common.Exceptions;
using AiTutor.Domain.Entities;
using AiTutor.Domain.Enums;
using AiTutor.UnitTests.Application;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AiTutor.UnitTests.Application.Classrooms;

public class ClassroomCreateTests : IDisposable
{
    private readonly TestDbContext _context;

    public ClassroomCreateTests()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new TestDbContext(options);
    }

    public void Dispose() => _context.Dispose();

    private async Task<User> CreateTeacherAsync()
    {
        var teacher = User.Create("Teacher", "Test", $"teacher{Guid.NewGuid():N}@test.com", UserRole.Teacher);
        _context.Users.Add(teacher);
        await _context.SaveChangesAsync();
        return teacher;
    }

    private async Task<Subscription> CreateActiveSubscriptionAsync(Guid userId)
    {
        var sub = Subscription.Create(userId, SubscriptionType.ParentMonthly,
            DateTime.UtcNow.AddDays(-5), DateTime.UtcNow.AddDays(25), 9.99m);
        _context.Subscriptions.Add(sub);
        await _context.SaveChangesAsync();
        return sub;
    }

    // ── CreateClassroom ────────────────────────────────────────────────
    [Fact]
    public async Task CreateClassroom_WithValidData_ShouldSucceed()
    {
        var teacher = await CreateTeacherAsync();
        await CreateActiveSubscriptionAsync(teacher.Id);
        var handler = new CreateClassroomCommandHandler(_context);
        var command = new CreateClassroomCommand("Math Class", "Description", SubjectType.Mathematics, teacher.Id);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(201);
        result.Data!.Name.Should().Be("Math Class");
        result.Data.TeacherId.Should().Be(teacher.Id);
    }

    [Fact]
    public async Task CreateClassroom_WithNonExistentTeacher_ShouldThrowNotFoundException()
    {
        var handler = new CreateClassroomCommandHandler(_context);
        var command = new CreateClassroomCommand("Class", "Desc", SubjectType.Mathematics, Guid.NewGuid());

        var act = async () => await handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CreateClassroom_WithNoSubscription_ShouldReturnFailure()
    {
        var teacher = await CreateTeacherAsync();
        var handler = new CreateClassroomCommandHandler(_context);
        var command = new CreateClassroomCommand("Class", "Desc", SubjectType.Mathematics, teacher.Id);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(403);
    }

    [Fact]
    public async Task CreateClassroom_WithMaxClassroomsReached_ShouldReturnFailure()
    {
        var teacher = await CreateTeacherAsync();
        await CreateActiveSubscriptionAsync(teacher.Id);
        for (int i = 0; i < 5; i++)
        {
            _context.Classrooms.Add(Classroom.Create($"Class {i}", "Desc", SubjectType.Mathematics, teacher.Id));
        }
        await _context.SaveChangesAsync();

        var handler = new CreateClassroomCommandHandler(_context);
        var command = new CreateClassroomCommand("6th Class", "Desc", SubjectType.Mathematics, teacher.Id);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(400);
    }

    // ── CreateClassroomCommandValidator ────────────────────────────────
    [Fact]
    public async Task CreateClassroomValidator_WithEmptyName_ShouldFail()
    {
        var validator = new CreateClassroomCommandValidator();
        var command = new CreateClassroomCommand("", "Desc", SubjectType.Mathematics, Guid.NewGuid());

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public async Task CreateClassroomValidator_WithNameTooLong_ShouldFail()
    {
        var validator = new CreateClassroomCommandValidator();
        var command = new CreateClassroomCommand(new string('A', 101), "Desc", SubjectType.Mathematics, Guid.NewGuid());

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public async Task CreateClassroomValidator_WithDescriptionTooLong_ShouldFail()
    {
        var validator = new CreateClassroomCommandValidator();
        var command = new CreateClassroomCommand("Name", new string('A', 501), SubjectType.Mathematics, Guid.NewGuid());

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Description");
    }

    [Fact]
    public async Task CreateClassroomValidator_WithEmptyTeacherId_ShouldFail()
    {
        var validator = new CreateClassroomCommandValidator();
        var command = new CreateClassroomCommand("Name", "Desc", SubjectType.Mathematics, Guid.Empty);

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "TeacherId");
    }

    [Fact]
    public async Task CreateClassroomValidator_WithValidData_ShouldPass()
    {
        var validator = new CreateClassroomCommandValidator();
        var command = new CreateClassroomCommand("Math", "Desc", SubjectType.Mathematics, Guid.NewGuid());

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
