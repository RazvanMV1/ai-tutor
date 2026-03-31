using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Features.Lessons.Commands.CreateLesson;
using AiTutor.Application.Features.Lessons.Queries.GetLessonById;
using AiTutor.Application.Features.Lessons.Queries.GetLessonsBySubject;
using AiTutor.Domain.Entities;
using AiTutor.Domain.Enums;
using AiTutor.UnitTests.Application;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AiTutor.UnitTests.Application.Lessons;

public class LessonTests : IDisposable
{
    private readonly TestDbContext _context;

    public LessonTests()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new TestDbContext(options);
    }

    public void Dispose() => _context.Dispose();

    private async Task<Subject> CreateSubjectAsync()
    {
        var subject = Subject.Create("Mathematics", "Math description", SubjectType.Mathematics);
        _context.Subjects.Add(subject);
        await _context.SaveChangesAsync();
        return subject;
    }

    private async Task<Lesson> CreateLessonAsync(Guid subjectId, string title = "Test Lesson", int order = 1)
    {
        var lesson = Lesson.Create(title, "Content here", order, DifficultyLevel.Beginner, subjectId);
        _context.Lessons.Add(lesson);
        await _context.SaveChangesAsync();
        return lesson;
    }

    // ── CreateLesson ───────────────────────────────────────────────────
    [Fact]
    public async Task CreateLesson_WithValidData_ShouldSucceed()
    {
        var subject = await CreateSubjectAsync();
        var handler = new CreateLessonCommandHandler(_context);
        var command = new CreateLessonCommand("Algebra Basics", "Content", 1, DifficultyLevel.Beginner, subject.Id);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(201);
        result.Data!.Title.Should().Be("Algebra Basics");
        result.Data.SubjectId.Should().Be(subject.Id);
    }

    [Fact]
    public async Task CreateLesson_WithNonExistentSubject_ShouldThrowNotFoundException()
    {
        var handler = new CreateLessonCommandHandler(_context);
        var command = new CreateLessonCommand("Lesson", "Content", 1, DifficultyLevel.Beginner, Guid.NewGuid());

        var act = async () => await handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    // ── CreateLessonCommandValidator ───────────────────────────────────
    [Fact]
    public async Task CreateLessonValidator_WithEmptyTitle_ShouldFail()
    {
        var validator = new CreateLessonCommandValidator();
        var command = new CreateLessonCommand("", "Content", 1, DifficultyLevel.Beginner, Guid.NewGuid());

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Title");
    }

    [Fact]
    public async Task CreateLessonValidator_WithTitleTooLong_ShouldFail()
    {
        var validator = new CreateLessonCommandValidator();
        var command = new CreateLessonCommand(new string('A', 201), "Content", 1, DifficultyLevel.Beginner, Guid.NewGuid());

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Title");
    }

    [Fact]
    public async Task CreateLessonValidator_WithEmptyContent_ShouldFail()
    {
        var validator = new CreateLessonCommandValidator();
        var command = new CreateLessonCommand("Title", "", 1, DifficultyLevel.Beginner, Guid.NewGuid());

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Content");
    }

    [Fact]
    public async Task CreateLessonValidator_WithZeroOrderIndex_ShouldFail()
    {
        var validator = new CreateLessonCommandValidator();
        var command = new CreateLessonCommand("Title", "Content", 0, DifficultyLevel.Beginner, Guid.NewGuid());

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "OrderIndex");
    }

    [Fact]
    public async Task CreateLessonValidator_WithEmptySubjectId_ShouldFail()
    {
        var validator = new CreateLessonCommandValidator();
        var command = new CreateLessonCommand("Title", "Content", 1, DifficultyLevel.Beginner, Guid.Empty);

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "SubjectId");
    }

    [Fact]
    public async Task CreateLessonValidator_WithValidData_ShouldPass()
    {
        var validator = new CreateLessonCommandValidator();
        var command = new CreateLessonCommand("Title", "Content", 1, DifficultyLevel.Beginner, Guid.NewGuid());

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    // ── GetLessonById ──────────────────────────────────────────────────
    [Fact]
    public async Task GetLessonById_WithValidId_ShouldReturnLesson()
    {
        var subject = await CreateSubjectAsync();
        var lesson = await CreateLessonAsync(subject.Id);
        var handler = new GetLessonByIdQueryHandler(_context);
        var query = new GetLessonByIdQuery(lesson.Id);

        var result = await handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Title.Should().Be("Test Lesson");
        result.Data.SubjectId.Should().Be(subject.Id);
    }

    [Fact]
    public async Task GetLessonById_WithInvalidId_ShouldThrowNotFoundException()
    {
        var handler = new GetLessonByIdQueryHandler(_context);
        var query = new GetLessonByIdQuery(Guid.NewGuid());

        var act = async () => await handler.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    // ── GetLessonsBySubject ────────────────────────────────────────────
    [Fact]
    public async Task GetLessonsBySubject_WithLessons_ShouldReturnOrderedList()
    {
        var subject = await CreateSubjectAsync();
        await CreateLessonAsync(subject.Id, "Lesson 2", 2);
        await CreateLessonAsync(subject.Id, "Lesson 1", 1);
        await CreateLessonAsync(subject.Id, "Lesson 3", 3);
        var handler = new GetLessonsBySubjectQueryHandler(_context);
        var query = new GetLessonsBySubjectQuery(subject.Id);

        var result = await handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Count.Should().Be(3);
        result.Data[0].Title.Should().Be("Lesson 1");
        result.Data[1].Title.Should().Be("Lesson 2");
        result.Data[2].Title.Should().Be("Lesson 3");
    }

    [Fact]
    public async Task GetLessonsBySubject_WithNoLessons_ShouldReturnEmptyList()
    {
        var handler = new GetLessonsBySubjectQueryHandler(_context);
        var query = new GetLessonsBySubjectQuery(Guid.NewGuid());

        var result = await handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Count.Should().Be(0);
    }
}
