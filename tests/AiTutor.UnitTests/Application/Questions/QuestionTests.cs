using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Features.Questions.Commands.CreateQuestion;
using AiTutor.Domain.Entities;
using AiTutor.Domain.Enums;
using AiTutor.UnitTests.Application;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AiTutor.UnitTests.Application.Questions;

public class QuestionTests : IDisposable
{
    private readonly TestDbContext _context;

    public QuestionTests()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new TestDbContext(options);
    }

    public void Dispose() => _context.Dispose();

    private async Task<Quiz> CreateFullQuizAsync()
    {
        var subject = Subject.Create("Math", "Desc", SubjectType.Mathematics);
        _context.Subjects.Add(subject);
        var lesson = Lesson.Create("Lesson", "Content", 1, DifficultyLevel.Beginner, subject.Id);
        _context.Lessons.Add(lesson);
        var quiz = Quiz.Create("Quiz", DifficultyLevel.Beginner, lesson.Id, 30);
        _context.Quizzes.Add(quiz);
        await _context.SaveChangesAsync();
        return quiz;
    }

    [Fact]
    public async Task CreateQuestion_WithValidData_ShouldSucceed()
    {
        var quiz = await CreateFullQuizAsync();
        var handler = new CreateQuestionCommandHandler(_context);
        var command = new CreateQuestionCommand("2+2=?", "4",
            new List<string> { "2", "3", "4", "5" }, 10, quiz.Id, "Basic math");

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(201);
        result.Data!.Text.Should().Be("2+2=?");
        result.Data.QuizId.Should().Be(quiz.Id);
    }

    [Fact]
    public async Task CreateQuestion_WithNonExistentQuiz_ShouldThrowNotFoundException()
    {
        var handler = new CreateQuestionCommandHandler(_context);
        var command = new CreateQuestionCommand("2+2=?", "4",
            new List<string> { "2", "3", "4", "5" }, 10, Guid.NewGuid());

        var act = async () => await handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    // ── CreateQuestionCommandValidator ─────────────────────────────────
    [Fact]
    public async Task CreateQuestionValidator_WithEmptyText_ShouldFail()
    {
        var validator = new CreateQuestionCommandValidator();
        var command = new CreateQuestionCommand("", "4",
            new List<string> { "2", "3", "4", "5" }, 10, Guid.NewGuid());

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Text");
    }

    [Fact]
    public async Task CreateQuestionValidator_WithTextTooLong_ShouldFail()
    {
        var validator = new CreateQuestionCommandValidator();
        var command = new CreateQuestionCommand(new string('A', 1001), "4",
            new List<string> { "2", "3", "4", "5" }, 10, Guid.NewGuid());

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Text");
    }

    [Fact]
    public async Task CreateQuestionValidator_WithEmptyCorrectAnswer_ShouldFail()
    {
        var validator = new CreateQuestionCommandValidator();
        var command = new CreateQuestionCommand("Q?", "",
            new List<string> { "2", "3", "4", "5" }, 10, Guid.NewGuid());

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "CorrectAnswer");
    }

    [Fact]
    public async Task CreateQuestionValidator_WithTooFewOptions_ShouldFail()
    {
        var validator = new CreateQuestionCommandValidator();
        var command = new CreateQuestionCommand("Q?", "4",
            new List<string> { "4" }, 10, Guid.NewGuid());

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task CreateQuestionValidator_WithTooManyOptions_ShouldFail()
    {
        var validator = new CreateQuestionCommandValidator();
        var command = new CreateQuestionCommand("Q?", "4",
            new List<string> { "1", "2", "3", "4", "5", "6", "7" }, 10, Guid.NewGuid());

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task CreateQuestionValidator_WithZeroPoints_ShouldFail()
    {
        var validator = new CreateQuestionCommandValidator();
        var command = new CreateQuestionCommand("Q?", "4",
            new List<string> { "2", "4" }, 0, Guid.NewGuid());

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Points");
    }

    [Fact]
    public async Task CreateQuestionValidator_WithPointsOver100_ShouldFail()
    {
        var validator = new CreateQuestionCommandValidator();
        var command = new CreateQuestionCommand("Q?", "4",
            new List<string> { "2", "4" }, 101, Guid.NewGuid());

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Points");
    }

    [Fact]
    public async Task CreateQuestionValidator_WithCorrectAnswerNotInOptions_ShouldFail()
    {
        var validator = new CreateQuestionCommandValidator();
        var command = new CreateQuestionCommand("Q?", "99",
            new List<string> { "2", "3", "4", "5" }, 10, Guid.NewGuid());

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task CreateQuestionValidator_WithValidData_ShouldPass()
    {
        var validator = new CreateQuestionCommandValidator();
        var command = new CreateQuestionCommand("Q?", "4",
            new List<string> { "2", "3", "4", "5" }, 10, Guid.NewGuid());

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }
}
