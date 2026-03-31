using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Features.Quizzes.Commands.CreateQuiz;
using AiTutor.Application.Features.Quizzes.Commands.SubmitQuiz;
using AiTutor.Application.Features.Quizzes.Queries.GetQuizById;
using AiTutor.Application.Features.Quizzes.Queries.GetQuizzesByLesson;
using AiTutor.Domain.Entities;
using AiTutor.Domain.Enums;
using AiTutor.UnitTests.Application;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AiTutor.UnitTests.Application.Quizzes;

public class QuizTests : IDisposable
{
    private readonly TestDbContext _context;

    public QuizTests()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new TestDbContext(options);
    }

    public void Dispose() => _context.Dispose();

    private async Task<Subject> CreateSubjectAsync()
    {
        var subject = Subject.Create("Math", "Desc", SubjectType.Mathematics);
        _context.Subjects.Add(subject);
        await _context.SaveChangesAsync();
        return subject;
    }

    private async Task<Lesson> CreateLessonAsync(Guid subjectId)
    {
        var lesson = Lesson.Create("Lesson", "Content", 1, DifficultyLevel.Beginner, subjectId);
        _context.Lessons.Add(lesson);
        await _context.SaveChangesAsync();
        return lesson;
    }

    private async Task<Quiz> CreateQuizAsync(Guid lessonId, string title = "Test Quiz")
    {
        var quiz = Quiz.Create(title, DifficultyLevel.Beginner, lessonId, 30);
        _context.Quizzes.Add(quiz);
        await _context.SaveChangesAsync();
        return quiz;
    }

    private async Task<Question> CreateQuestionAsync(Guid quizId, string correctAnswer = "4")
    {
        var question = Question.Create("2+2=?", correctAnswer,
            new List<string> { "2", "3", "4", "5" }, 10, quizId, "Basic math");
        _context.Questions.Add(question);
        await _context.SaveChangesAsync();
        return question;
    }

    // ── CreateQuiz ─────────────────────────────────────────────────────
    [Fact]
    public async Task CreateQuiz_WithValidData_ShouldSucceed()
    {
        var subject = await CreateSubjectAsync();
        var lesson = await CreateLessonAsync(subject.Id);
        var handler = new CreateQuizCommandHandler(_context);
        var command = new CreateQuizCommand("Quiz 1", DifficultyLevel.Intermediate, lesson.Id, 45);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(201);
        result.Data!.Title.Should().Be("Quiz 1");
        result.Data.TimeLimitMinutes.Should().Be(45);
    }

    [Fact]
    public async Task CreateQuiz_WithNonExistentLesson_ShouldThrowNotFoundException()
    {
        var handler = new CreateQuizCommandHandler(_context);
        var command = new CreateQuizCommand("Quiz", DifficultyLevel.Beginner, Guid.NewGuid(), 30);

        var act = async () => await handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    // ── CreateQuizCommandValidator ─────────────────────────────────────
    [Fact]
    public async Task CreateQuizValidator_WithEmptyTitle_ShouldFail()
    {
        var validator = new CreateQuizCommandValidator();
        var command = new CreateQuizCommand("", DifficultyLevel.Beginner, Guid.NewGuid(), 30);

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Title");
    }

    [Fact]
    public async Task CreateQuizValidator_WithTitleTooLong_ShouldFail()
    {
        var validator = new CreateQuizCommandValidator();
        var command = new CreateQuizCommand(new string('A', 201), DifficultyLevel.Beginner, Guid.NewGuid(), 30);

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Title");
    }

    [Fact]
    public async Task CreateQuizValidator_WithEmptyLessonId_ShouldFail()
    {
        var validator = new CreateQuizCommandValidator();
        var command = new CreateQuizCommand("Quiz", DifficultyLevel.Beginner, Guid.Empty, 30);

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "LessonId");
    }

    [Fact]
    public async Task CreateQuizValidator_WithZeroTimeLimit_ShouldFail()
    {
        var validator = new CreateQuizCommandValidator();
        var command = new CreateQuizCommand("Quiz", DifficultyLevel.Beginner, Guid.NewGuid(), 0);

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "TimeLimitMinutes");
    }

    [Fact]
    public async Task CreateQuizValidator_WithTimeLimitOver180_ShouldFail()
    {
        var validator = new CreateQuizCommandValidator();
        var command = new CreateQuizCommand("Quiz", DifficultyLevel.Beginner, Guid.NewGuid(), 181);

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "TimeLimitMinutes");
    }

    [Fact]
    public async Task CreateQuizValidator_WithValidData_ShouldPass()
    {
        var validator = new CreateQuizCommandValidator();
        var command = new CreateQuizCommand("Quiz", DifficultyLevel.Beginner, Guid.NewGuid(), 30);

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    // ── SubmitQuiz ─────────────────────────────────────────────────────
    [Fact]
    public async Task SubmitQuiz_WithCorrectAnswers_ShouldReturn100Percent()
    {
        var subject = await CreateSubjectAsync();
        var lesson = await CreateLessonAsync(subject.Id);
        var quiz = await CreateQuizAsync(lesson.Id);
        var question = await CreateQuestionAsync(quiz.Id, "4");
        var handler = new SubmitQuizCommandHandler(_context);
        var userId = Guid.NewGuid();
        var command = new SubmitQuizCommand(quiz.Id, userId,
            new List<QuizAnswerDto> { new QuizAnswerDto(question.Id, "4") });

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.CorrectAnswers.Should().Be(1);
        result.Data.TotalQuestions.Should().Be(1);
        result.Data.ScorePercentage.Should().Be(100);
        result.Data.Results[0].IsCorrect.Should().BeTrue();
    }

    [Fact]
    public async Task SubmitQuiz_WithWrongAnswers_ShouldReturn0Percent()
    {
        var subject = await CreateSubjectAsync();
        var lesson = await CreateLessonAsync(subject.Id);
        var quiz = await CreateQuizAsync(lesson.Id);
        var question = await CreateQuestionAsync(quiz.Id, "4");
        var handler = new SubmitQuizCommandHandler(_context);
        var command = new SubmitQuizCommand(quiz.Id, Guid.NewGuid(),
            new List<QuizAnswerDto> { new QuizAnswerDto(question.Id, "2") });

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.CorrectAnswers.Should().Be(0);
        result.Data.ScorePercentage.Should().Be(0);
        result.Data.Results[0].IsCorrect.Should().BeFalse();
    }

    [Fact]
    public async Task SubmitQuiz_WithNonExistentQuiz_ShouldThrowNotFoundException()
    {
        var handler = new SubmitQuizCommandHandler(_context);
        var command = new SubmitQuizCommand(Guid.NewGuid(), Guid.NewGuid(),
            new List<QuizAnswerDto> { new QuizAnswerDto(Guid.NewGuid(), "4") });

        var act = async () => await handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task SubmitQuiz_WithNonExistentQuestion_ShouldSkipIt()
    {
        var subject = await CreateSubjectAsync();
        var lesson = await CreateLessonAsync(subject.Id);
        var quiz = await CreateQuizAsync(lesson.Id);
        await CreateQuestionAsync(quiz.Id);
        var handler = new SubmitQuizCommandHandler(_context);
        var command = new SubmitQuizCommand(quiz.Id, Guid.NewGuid(),
            new List<QuizAnswerDto> { new QuizAnswerDto(Guid.NewGuid(), "4") });

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Results.Count.Should().Be(0);
    }

    // ── SubmitQuizCommandValidator ─────────────────────────────────────
    [Fact]
    public async Task SubmitQuizValidator_WithEmptyQuizId_ShouldFail()
    {
        var validator = new SubmitQuizCommandValidator();
        var command = new SubmitQuizCommand(Guid.Empty, Guid.NewGuid(),
            new List<QuizAnswerDto> { new QuizAnswerDto(Guid.NewGuid(), "4") });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "QuizId");
    }

    [Fact]
    public async Task SubmitQuizValidator_WithEmptyUserId_ShouldFail()
    {
        var validator = new SubmitQuizCommandValidator();
        var command = new SubmitQuizCommand(Guid.NewGuid(), Guid.Empty,
            new List<QuizAnswerDto> { new QuizAnswerDto(Guid.NewGuid(), "4") });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "UserId");
    }

    [Fact]
    public async Task SubmitQuizValidator_WithEmptyAnswers_ShouldFail()
    {
        var validator = new SubmitQuizCommandValidator();
        var command = new SubmitQuizCommand(Guid.NewGuid(), Guid.NewGuid(), new List<QuizAnswerDto>());

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Answers");
    }

    [Fact]
    public async Task SubmitQuizValidator_WithValidData_ShouldPass()
    {
        var validator = new SubmitQuizCommandValidator();
        var command = new SubmitQuizCommand(Guid.NewGuid(), Guid.NewGuid(),
            new List<QuizAnswerDto> { new QuizAnswerDto(Guid.NewGuid(), "4") });

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    // ── GetQuizById ────────────────────────────────────────────────────
    [Fact]
    public async Task GetQuizById_WithValidId_ShouldReturnQuizWithQuestions()
    {
        var subject = await CreateSubjectAsync();
        var lesson = await CreateLessonAsync(subject.Id);
        var quiz = await CreateQuizAsync(lesson.Id);
        await CreateQuestionAsync(quiz.Id);
        var handler = new GetQuizByIdQueryHandler(_context);
        var query = new GetQuizByIdQuery(quiz.Id);

        var result = await handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Title.Should().Be("Test Quiz");
        result.Data.Questions.Count.Should().Be(1);
    }

    [Fact]
    public async Task GetQuizById_WithInvalidId_ShouldThrowNotFoundException()
    {
        var handler = new GetQuizByIdQueryHandler(_context);
        var query = new GetQuizByIdQuery(Guid.NewGuid());

        var act = async () => await handler.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    // ── GetQuizzesByLesson ─────────────────────────────────────────────
    [Fact]
    public async Task GetQuizzesByLesson_WithQuizzes_ShouldReturnList()
    {
        var subject = await CreateSubjectAsync();
        var lesson = await CreateLessonAsync(subject.Id);
        await CreateQuizAsync(lesson.Id, "Quiz A");
        await CreateQuizAsync(lesson.Id, "Quiz B");
        var handler = new GetQuizzesByLessonQueryHandler(_context);
        var query = new GetQuizzesByLessonQuery(lesson.Id);

        var result = await handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Count.Should().Be(2);
    }

    [Fact]
    public async Task GetQuizzesByLesson_WithNoQuizzes_ShouldReturnEmptyList()
    {
        var handler = new GetQuizzesByLessonQueryHandler(_context);
        var query = new GetQuizzesByLessonQuery(Guid.NewGuid());

        var result = await handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Count.Should().Be(0);
    }
}
