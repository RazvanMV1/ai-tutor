using AiTutor.Application.Features.Classrooms.Commands.CreateClassroom;
using AiTutor.Application.Features.Lessons.Commands.CreateLesson;
using AiTutor.Application.Features.Progress.Commands.CompleteLesson;
using AiTutor.Application.Features.Questions.Commands.CreateQuestion;
using AiTutor.Application.Features.Quizzes.Commands.CreateQuiz;
using AiTutor.Application.Features.Quizzes.Commands.SubmitQuiz;
using AiTutor.Application.Features.Subjects.Commands.CreateSubject;
using AiTutor.Application.Features.Subscriptions.Commands.CreateSubscription;
using AiTutor.Domain.Enums;
using FluentValidation.TestHelper;
using FluentAssertions;
using Xunit;

namespace AiTutor.UnitTests.Application.Common;

public class AllValidatorsTests
{
    // ── CreateClassroomCommandValidator ─────────────────────────────

    [Fact]
    public async Task CreateClassroom_Valid_ShouldPass()
    {
        var v = new CreateClassroomCommandValidator();
        var cmd = new CreateClassroomCommand("Math 9A", "Description", SubjectType.Mathematics, Guid.NewGuid());
        var result = await v.TestValidateAsync(cmd);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task CreateClassroom_EmptyName_ShouldFail()
    {
        var v = new CreateClassroomCommandValidator();
        var cmd = new CreateClassroomCommand("", "d", SubjectType.Mathematics, Guid.NewGuid());
        var result = await v.TestValidateAsync(cmd);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public async Task CreateClassroom_TooLongName_ShouldFail()
    {
        var v = new CreateClassroomCommandValidator();
        var cmd = new CreateClassroomCommand(new string('x', 101), "d", SubjectType.Mathematics, Guid.NewGuid());
        var result = await v.TestValidateAsync(cmd);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public async Task CreateClassroom_TooLongDescription_ShouldFail()
    {
        var v = new CreateClassroomCommandValidator();
        var cmd = new CreateClassroomCommand("Name", new string('x', 501), SubjectType.Mathematics, Guid.NewGuid());
        var result = await v.TestValidateAsync(cmd);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public async Task CreateClassroom_EmptyTeacherId_ShouldFail()
    {
        var v = new CreateClassroomCommandValidator();
        var cmd = new CreateClassroomCommand("Name", "d", SubjectType.Mathematics, Guid.Empty);
        var result = await v.TestValidateAsync(cmd);
        result.ShouldHaveValidationErrorFor(x => x.TeacherId);
    }

    // ── CreateLessonCommandValidator ────────────────────────────────

    [Fact]
    public async Task CreateLesson_Valid_ShouldPass()
    {
        var v = new CreateLessonCommandValidator();
        var cmd = new CreateLessonCommand("Title", "Content", 1, DifficultyLevel.Beginner, Guid.NewGuid());
        var result = await v.TestValidateAsync(cmd);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task CreateLesson_EmptyTitle_ShouldFail()
    {
        var v = new CreateLessonCommandValidator();
        var cmd = new CreateLessonCommand("", "Content", 1, DifficultyLevel.Beginner, Guid.NewGuid());
        var result = await v.TestValidateAsync(cmd);
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public async Task CreateLesson_TooLongTitle_ShouldFail()
    {
        var v = new CreateLessonCommandValidator();
        var cmd = new CreateLessonCommand(new string('x', 201), "Content", 1, DifficultyLevel.Beginner, Guid.NewGuid());
        var result = await v.TestValidateAsync(cmd);
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public async Task CreateLesson_EmptyContent_ShouldFail()
    {
        var v = new CreateLessonCommandValidator();
        var cmd = new CreateLessonCommand("Title", "", 1, DifficultyLevel.Beginner, Guid.NewGuid());
        var result = await v.TestValidateAsync(cmd);
        result.ShouldHaveValidationErrorFor(x => x.Content);
    }

    [Fact]
    public async Task CreateLesson_ZeroOrderIndex_ShouldFail()
    {
        var v = new CreateLessonCommandValidator();
        var cmd = new CreateLessonCommand("Title", "Content", 0, DifficultyLevel.Beginner, Guid.NewGuid());
        var result = await v.TestValidateAsync(cmd);
        result.ShouldHaveValidationErrorFor(x => x.OrderIndex);
    }

    [Fact]
    public async Task CreateLesson_EmptySubjectId_ShouldFail()
    {
        var v = new CreateLessonCommandValidator();
        var cmd = new CreateLessonCommand("Title", "Content", 1, DifficultyLevel.Beginner, Guid.Empty);
        var result = await v.TestValidateAsync(cmd);
        result.ShouldHaveValidationErrorFor(x => x.SubjectId);
    }

    // ── CompleteLessonCommandValidator ──────────────────────────────

    [Fact]
    public async Task CompleteLesson_Valid_ShouldPass()
    {
        var v = new CompleteLessonCommandValidator();
        var cmd = new CompleteLessonCommand(Guid.NewGuid(), Guid.NewGuid(), 80);
        var result = await v.TestValidateAsync(cmd);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task CompleteLesson_EmptyUserId_ShouldFail()
    {
        var v = new CompleteLessonCommandValidator();
        var cmd = new CompleteLessonCommand(Guid.Empty, Guid.NewGuid(), 80);
        var result = await v.TestValidateAsync(cmd);
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public async Task CompleteLesson_EmptyLessonId_ShouldFail()
    {
        var v = new CompleteLessonCommandValidator();
        var cmd = new CompleteLessonCommand(Guid.NewGuid(), Guid.Empty, 80);
        var result = await v.TestValidateAsync(cmd);
        result.ShouldHaveValidationErrorFor(x => x.LessonId);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    public async Task CompleteLesson_OutOfRangeScore_ShouldFail(int score)
    {
        var v = new CompleteLessonCommandValidator();
        var cmd = new CompleteLessonCommand(Guid.NewGuid(), Guid.NewGuid(), score);
        var result = await v.TestValidateAsync(cmd);
        result.ShouldHaveValidationErrorFor(x => x.ScorePercentage);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(50)]
    [InlineData(100)]
    public async Task CompleteLesson_ValidScore_ShouldPass(int score)
    {
        var v = new CompleteLessonCommandValidator();
        var cmd = new CompleteLessonCommand(Guid.NewGuid(), Guid.NewGuid(), score);
        var result = await v.TestValidateAsync(cmd);
        result.ShouldNotHaveValidationErrorFor(x => x.ScorePercentage);
    }

    // ── CreateQuestionCommandValidator ──────────────────────────────

    [Fact]
    public async Task CreateQuestion_Valid_ShouldPass()
    {
        var v = new CreateQuestionCommandValidator();
        var cmd = new CreateQuestionCommand("Q?", "A", new List<string> { "A", "B" }, 10, Guid.NewGuid());
        var result = await v.TestValidateAsync(cmd);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task CreateQuestion_EmptyText_ShouldFail()
    {
        var v = new CreateQuestionCommandValidator();
        var cmd = new CreateQuestionCommand("", "A", new List<string> { "A", "B" }, 10, Guid.NewGuid());
        var result = await v.TestValidateAsync(cmd);
        result.ShouldHaveValidationErrorFor(x => x.Text);
    }

    [Fact]
    public async Task CreateQuestion_TooLongText_ShouldFail()
    {
        var v = new CreateQuestionCommandValidator();
        var cmd = new CreateQuestionCommand(new string('x', 1001), "A",
            new List<string> { "A", "B" }, 10, Guid.NewGuid());
        var result = await v.TestValidateAsync(cmd);
        result.ShouldHaveValidationErrorFor(x => x.Text);
    }

    [Fact]
    public async Task CreateQuestion_EmptyCorrectAnswer_ShouldFail()
    {
        var v = new CreateQuestionCommandValidator();
        var cmd = new CreateQuestionCommand("Q?", "", new List<string> { "A", "B" }, 10, Guid.NewGuid());
        var result = await v.TestValidateAsync(cmd);
        result.ShouldHaveValidationErrorFor(x => x.CorrectAnswer);
    }

    [Fact]
    public async Task CreateQuestion_TooFewOptions_ShouldFail()
    {
        var v = new CreateQuestionCommandValidator();
        var cmd = new CreateQuestionCommand("Q?", "A", new List<string> { "A" }, 10, Guid.NewGuid());
        var result = await v.TestValidateAsync(cmd);
        result.ShouldHaveValidationErrorFor(x => x.Options);
    }

    [Fact]
    public async Task CreateQuestion_TooManyOptions_ShouldFail()
    {
        var v = new CreateQuestionCommandValidator();
        var cmd = new CreateQuestionCommand("Q?", "A",
            new List<string> { "A", "B", "C", "D", "E", "F", "G" }, 10, Guid.NewGuid());
        var result = await v.TestValidateAsync(cmd);
        result.ShouldHaveValidationErrorFor(x => x.Options);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(101)]
    public async Task CreateQuestion_InvalidPoints_ShouldFail(int points)
    {
        var v = new CreateQuestionCommandValidator();
        var cmd = new CreateQuestionCommand("Q?", "A", new List<string> { "A", "B" }, points, Guid.NewGuid());
        var result = await v.TestValidateAsync(cmd);
        result.ShouldHaveValidationErrorFor(x => x.Points);
    }

    [Fact]
    public async Task CreateQuestion_CorrectAnswerNotInOptions_ShouldFail()
    {
        var v = new CreateQuestionCommandValidator();
        var cmd = new CreateQuestionCommand("Q?", "Z", new List<string> { "A", "B" }, 10, Guid.NewGuid());
        var result = await v.TestValidateAsync(cmd);
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CreateQuestion_EmptyQuizId_ShouldFail()
    {
        var v = new CreateQuestionCommandValidator();
        var cmd = new CreateQuestionCommand("Q?", "A", new List<string> { "A", "B" }, 10, Guid.Empty);
        var result = await v.TestValidateAsync(cmd);
        result.ShouldHaveValidationErrorFor(x => x.QuizId);
    }

    // ── CreateQuizCommandValidator ──────────────────────────────────

    [Fact]
    public async Task CreateQuiz_Valid_ShouldPass()
    {
        var v = new CreateQuizCommandValidator();
        var cmd = new CreateQuizCommand("Quiz", DifficultyLevel.Beginner, Guid.NewGuid(), 30);
        var result = await v.TestValidateAsync(cmd);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task CreateQuiz_EmptyTitle_ShouldFail()
    {
        var v = new CreateQuizCommandValidator();
        var cmd = new CreateQuizCommand("", DifficultyLevel.Beginner, Guid.NewGuid(), 30);
        var result = await v.TestValidateAsync(cmd);
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public async Task CreateQuiz_TooLongTitle_ShouldFail()
    {
        var v = new CreateQuizCommandValidator();
        var cmd = new CreateQuizCommand(new string('x', 201), DifficultyLevel.Beginner, Guid.NewGuid(), 30);
        var result = await v.TestValidateAsync(cmd);
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public async Task CreateQuiz_EmptyLessonId_ShouldFail()
    {
        var v = new CreateQuizCommandValidator();
        var cmd = new CreateQuizCommand("Quiz", DifficultyLevel.Beginner, Guid.Empty, 30);
        var result = await v.TestValidateAsync(cmd);
        result.ShouldHaveValidationErrorFor(x => x.LessonId);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(181)]
    public async Task CreateQuiz_InvalidTimeLimit_ShouldFail(int minutes)
    {
        var v = new CreateQuizCommandValidator();
        var cmd = new CreateQuizCommand("Quiz", DifficultyLevel.Beginner, Guid.NewGuid(), minutes);
        var result = await v.TestValidateAsync(cmd);
        result.ShouldHaveValidationErrorFor(x => x.TimeLimitMinutes);
    }

    // ── SubmitQuizCommandValidator ──────────────────────────────────

    [Fact]
    public async Task SubmitQuiz_Valid_ShouldPass()
    {
        var v = new SubmitQuizCommandValidator();
        var cmd = new SubmitQuizCommand(Guid.NewGuid(), Guid.NewGuid(),
            new List<QuizAnswerDto> { new(Guid.NewGuid(), "A") });
        var result = await v.TestValidateAsync(cmd);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task SubmitQuiz_EmptyQuizId_ShouldFail()
    {
        var v = new SubmitQuizCommandValidator();
        var cmd = new SubmitQuizCommand(Guid.Empty, Guid.NewGuid(),
            new List<QuizAnswerDto> { new(Guid.NewGuid(), "A") });
        var result = await v.TestValidateAsync(cmd);
        result.ShouldHaveValidationErrorFor(x => x.QuizId);
    }

    [Fact]
    public async Task SubmitQuiz_EmptyUserId_ShouldFail()
    {
        var v = new SubmitQuizCommandValidator();
        var cmd = new SubmitQuizCommand(Guid.NewGuid(), Guid.Empty,
            new List<QuizAnswerDto> { new(Guid.NewGuid(), "A") });
        var result = await v.TestValidateAsync(cmd);
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public async Task SubmitQuiz_EmptyAnswers_ShouldFail()
    {
        var v = new SubmitQuizCommandValidator();
        var cmd = new SubmitQuizCommand(Guid.NewGuid(), Guid.NewGuid(), new List<QuizAnswerDto>());
        var result = await v.TestValidateAsync(cmd);
        result.ShouldHaveValidationErrorFor(x => x.Answers);
    }

    // ── CreateSubjectCommandValidator ───────────────────────────────

    [Fact]
    public async Task CreateSubject_Valid_ShouldPass()
    {
        var v = new CreateSubjectCommandValidator();
        var cmd = new CreateSubjectCommand("Math", "Description", SubjectType.Mathematics);
        var result = await v.TestValidateAsync(cmd);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task CreateSubject_EmptyName_ShouldFail()
    {
        var v = new CreateSubjectCommandValidator();
        var cmd = new CreateSubjectCommand("", "Description", SubjectType.Mathematics);
        var result = await v.TestValidateAsync(cmd);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public async Task CreateSubject_TooLongName_ShouldFail()
    {
        var v = new CreateSubjectCommandValidator();
        var cmd = new CreateSubjectCommand(new string('x', 101), "Description", SubjectType.Mathematics);
        var result = await v.TestValidateAsync(cmd);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public async Task CreateSubject_EmptyDescription_ShouldFail()
    {
        var v = new CreateSubjectCommandValidator();
        var cmd = new CreateSubjectCommand("Math", "", SubjectType.Mathematics);
        var result = await v.TestValidateAsync(cmd);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public async Task CreateSubject_TooLongDescription_ShouldFail()
    {
        var v = new CreateSubjectCommandValidator();
        var cmd = new CreateSubjectCommand("Math", new string('x', 501), SubjectType.Mathematics);
        var result = await v.TestValidateAsync(cmd);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    // ── CreateSubscriptionCommandValidator ──────────────────────────

    [Fact]
    public async Task CreateSubscription_Valid_ShouldPass()
    {
        var v = new CreateSubscriptionCommandValidator();
        var cmd = new CreateSubscriptionCommand(Guid.NewGuid(), SubscriptionType.ParentMonthly,
            49.99m, DateTime.UtcNow, DateTime.UtcNow.AddDays(30));
        var result = await v.TestValidateAsync(cmd);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task CreateSubscription_EmptyUserId_ShouldFail()
    {
        var v = new CreateSubscriptionCommandValidator();
        var cmd = new CreateSubscriptionCommand(Guid.Empty, SubscriptionType.ParentMonthly,
            49.99m, DateTime.UtcNow, DateTime.UtcNow.AddDays(30));
        var result = await v.TestValidateAsync(cmd);
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public async Task CreateSubscription_ZeroPrice_ShouldFail()
    {
        var v = new CreateSubscriptionCommandValidator();
        var cmd = new CreateSubscriptionCommand(Guid.NewGuid(), SubscriptionType.ParentMonthly,
            0m, DateTime.UtcNow, DateTime.UtcNow.AddDays(30));
        var result = await v.TestValidateAsync(cmd);
        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [Fact]
    public async Task CreateSubscription_StartAfterEnd_ShouldFail()
    {
        var v = new CreateSubscriptionCommandValidator();
        var cmd = new CreateSubscriptionCommand(Guid.NewGuid(), SubscriptionType.ParentMonthly,
            49.99m, DateTime.UtcNow.AddDays(30), DateTime.UtcNow.AddDays(15));
        var result = await v.TestValidateAsync(cmd);
        result.ShouldHaveValidationErrorFor(x => x.StartDate);
    }

    [Fact]
    public async Task CreateSubscription_EndDateInPast_ShouldFail()
    {
        var v = new CreateSubscriptionCommandValidator();
        var cmd = new CreateSubscriptionCommand(Guid.NewGuid(), SubscriptionType.ParentMonthly,
            49.99m, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow.AddDays(-1));
        var result = await v.TestValidateAsync(cmd);
        result.ShouldHaveValidationErrorFor(x => x.EndDate);
    }
}
