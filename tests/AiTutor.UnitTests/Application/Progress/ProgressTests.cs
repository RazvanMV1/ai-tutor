using AiTutor.Application.Features.Progress.Commands.CompleteLesson;
using AiTutor.Application.Features.Progress.Queries.GetStudentProgress;
using AiTutor.Domain.Entities;
using AiTutor.Domain.Enums;
using AiTutor.UnitTests.Application;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AiTutor.UnitTests.Application.Progress;

public class ProgressTests : IDisposable
{
    private readonly TestDbContext _context;

    public ProgressTests()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new TestDbContext(options);
    }

    public void Dispose() => _context.Dispose();

    private async Task<(User user, Lesson lesson)> CreateUserAndLessonAsync()
    {
        var user = User.Create("Test", "User", $"test{Guid.NewGuid():N}@test.com", UserRole.Student);
        _context.Users.Add(user);
        var subject = Subject.Create("Math", "Desc", SubjectType.Mathematics);
        _context.Subjects.Add(subject);
        var lesson = Lesson.Create("Lesson", "Content", 1, DifficultyLevel.Beginner, subject.Id);
        _context.Lessons.Add(lesson);
        await _context.SaveChangesAsync();
        return (user, lesson);
    }

    // ── CompleteLesson ─────────────────────────────────────────────────
    [Fact]
    public async Task CompleteLesson_NewProgress_ShouldCreateAndComplete()
    {
        var (user, lesson) = await CreateUserAndLessonAsync();
        var handler = new CompleteLessonCommandHandler(_context);
        var command = new CompleteLessonCommand(user.Id, lesson.Id, 85);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.IsCompleted.Should().BeTrue();
        result.Data.ScorePercentage.Should().Be(85);
        result.Data.CompletedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task CompleteLesson_ExistingProgress_ShouldUpdateExisting()
    {
        var (user, lesson) = await CreateUserAndLessonAsync();
        var progress = StudentProgress.Create(user.Id, lesson.Id);
        _context.StudentProgresses.Add(progress);
        await _context.SaveChangesAsync();

        var handler = new CompleteLessonCommandHandler(_context);
        var command = new CompleteLessonCommand(user.Id, lesson.Id, 90);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.IsCompleted.Should().BeTrue();
        result.Data.ScorePercentage.Should().Be(90);
    }

    // ── CompleteLessonCommandValidator ─────────────────────────────────
    [Fact]
    public async Task CompleteLessonValidator_WithEmptyUserId_ShouldFail()
    {
        var validator = new CompleteLessonCommandValidator();
        var command = new CompleteLessonCommand(Guid.Empty, Guid.NewGuid(), 50);

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "UserId");
    }

    [Fact]
    public async Task CompleteLessonValidator_WithEmptyLessonId_ShouldFail()
    {
        var validator = new CompleteLessonCommandValidator();
        var command = new CompleteLessonCommand(Guid.NewGuid(), Guid.Empty, 50);

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "LessonId");
    }

    [Fact]
    public async Task CompleteLessonValidator_WithScoreTooLow_ShouldFail()
    {
        var validator = new CompleteLessonCommandValidator();
        var command = new CompleteLessonCommand(Guid.NewGuid(), Guid.NewGuid(), -1);

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "ScorePercentage");
    }

    [Fact]
    public async Task CompleteLessonValidator_WithScoreTooHigh_ShouldFail()
    {
        var validator = new CompleteLessonCommandValidator();
        var command = new CompleteLessonCommand(Guid.NewGuid(), Guid.NewGuid(), 101);

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "ScorePercentage");
    }

    [Fact]
    public async Task CompleteLessonValidator_WithValidData_ShouldPass()
    {
        var validator = new CompleteLessonCommandValidator();
        var command = new CompleteLessonCommand(Guid.NewGuid(), Guid.NewGuid(), 75);

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    // ── GetStudentProgress ─────────────────────────────────────────────
    [Fact]
    public async Task GetStudentProgress_WithProgress_ShouldReturnList()
    {
        var (user, lesson) = await CreateUserAndLessonAsync();
        var progress = StudentProgress.Create(user.Id, lesson.Id);
        progress.Complete(80);
        _context.StudentProgresses.Add(progress);
        await _context.SaveChangesAsync();

        var handler = new GetStudentProgressQueryHandler(_context);
        var query = new GetStudentProgressQuery(user.Id);

        var result = await handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Count.Should().Be(1);
        result.Data[0].LessonTitle.Should().Be("Lesson");
        result.Data[0].IsCompleted.Should().BeTrue();
    }

    [Fact]
    public async Task GetStudentProgress_WithNoProgress_ShouldReturnEmptyList()
    {
        var handler = new GetStudentProgressQueryHandler(_context);
        var query = new GetStudentProgressQuery(Guid.NewGuid());

        var result = await handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Count.Should().Be(0);
    }
}
