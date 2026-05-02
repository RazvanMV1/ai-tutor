using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Features.Classrooms.Commands.CreateClassroomLesson;
using AiTutor.Application.Features.Classrooms.Commands.CreateClassroomQuiz;
using AiTutor.Application.Features.Classrooms.Commands.DeleteClassroomLesson;
using AiTutor.Application.Features.Classrooms.Commands.DeleteClassroomQuestion;
using AiTutor.Application.Features.Classrooms.Commands.DeleteClassroomQuiz;
using AiTutor.Application.Features.Classrooms.Commands.UpdateClassroomLesson;
using AiTutor.Application.Features.Classrooms.Commands.UpdateClassroomQuiz;
using AiTutor.Domain.Entities;
using AiTutor.Domain.Enums;
using AiTutor.UnitTests.Application;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AiTutor.UnitTests.Application.Classrooms;

public class ClassroomLessonQuizDeleteUpdateTests : IDisposable
{
    private readonly TestDbContext _context;

    public ClassroomLessonQuizDeleteUpdateTests()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new TestDbContext(options);
    }

    public void Dispose() => _context.Dispose();

    private async Task<User> CreateUserAsync(UserRole role = UserRole.Teacher)
    {
        var user = User.Create("Test", "User", $"u{Guid.NewGuid():N}@t.com", role);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    private async Task<Classroom> CreateClassroomAsync(Guid teacherId)
    {
        var c = Classroom.Create("Class", "Desc", SubjectType.Mathematics, teacherId);
        _context.Classrooms.Add(c);
        await _context.SaveChangesAsync();
        return c;
    }

    private async Task<ClassroomLesson> CreateLessonAsync(Guid classroomId)
    {
        var l = ClassroomLesson.Create(classroomId, "L", "content", 0, DifficultyLevel.Beginner);
        _context.ClassroomLessons.Add(l);
        await _context.SaveChangesAsync();
        return l;
    }

    private async Task<ClassroomQuiz> CreateQuizAsync(Guid classroomId, Guid lessonId)
    {
        var q = ClassroomQuiz.Create(classroomId, lessonId, "Q", DifficultyLevel.Beginner, 15);
        _context.ClassroomQuizzes.Add(q);
        await _context.SaveChangesAsync();
        return q;
    }

    // â”€â”€ DeleteClassroomLesson â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

    [Fact]
    public async Task DeleteLesson_WithValidData_ShouldRemoveLesson()
    {
        var teacher = await CreateUserAsync();
        var classroom = await CreateClassroomAsync(teacher.Id);
        var lesson = await CreateLessonAsync(classroom.Id);
        var handler = new DeleteClassroomLessonCommandHandler(_context);

        var result = await handler.Handle(
            new DeleteClassroomLessonCommand(classroom.Id, lesson.Id, teacher.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        (await _context.ClassroomLessons.CountAsync()).Should().Be(0);
    }

    [Fact]
    public async Task DeleteLesson_WithNonExistentClassroom_ShouldThrowNotFound()
    {
        var handler = new DeleteClassroomLessonCommandHandler(_context);
        var act = async () => await handler.Handle(
            new DeleteClassroomLessonCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()),
            CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task DeleteLesson_WithNonTeacher_ShouldThrowForbidden()
    {
        var teacher = await CreateUserAsync();
        var classroom = await CreateClassroomAsync(teacher.Id);
        var handler = new DeleteClassroomLessonCommandHandler(_context);

        var act = async () => await handler.Handle(
            new DeleteClassroomLessonCommand(classroom.Id, Guid.NewGuid(), Guid.NewGuid()),
            CancellationToken.None);

        await act.Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Fact]
    public async Task DeleteLesson_WithNonExistentLesson_ShouldThrowNotFound()
    {
        var teacher = await CreateUserAsync();
        var classroom = await CreateClassroomAsync(teacher.Id);
        var handler = new DeleteClassroomLessonCommandHandler(_context);

        var act = async () => await handler.Handle(
            new DeleteClassroomLessonCommand(classroom.Id, Guid.NewGuid(), teacher.Id),
            CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    // â”€â”€ UpdateClassroomLesson â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

    [Fact]
    public async Task UpdateLesson_WithValidData_ShouldUpdateFields()
    {
        var teacher = await CreateUserAsync();
        var classroom = await CreateClassroomAsync(teacher.Id);
        var lesson = await CreateLessonAsync(classroom.Id);
        var handler = new UpdateClassroomLessonCommandHandler(_context);

        var result = await handler.Handle(new UpdateClassroomLessonCommand(
            classroom.Id, lesson.Id, teacher.Id, "New Title", "new content", DifficultyLevel.Advanced
        ), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Title.Should().Be("New Title");
        result.Data.Difficulty.Should().Be(DifficultyLevel.Advanced);
    }

    [Fact]
    public async Task UpdateLesson_WithNonExistentClassroom_ShouldThrowNotFound()
    {
        var handler = new UpdateClassroomLessonCommandHandler(_context);
        var act = async () => await handler.Handle(new UpdateClassroomLessonCommand(
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "x", "y", DifficultyLevel.Beginner
        ), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task UpdateLesson_WithNonTeacher_ShouldThrowForbidden()
    {
        var teacher = await CreateUserAsync();
        var classroom = await CreateClassroomAsync(teacher.Id);
        var handler = new UpdateClassroomLessonCommandHandler(_context);

        var act = async () => await handler.Handle(new UpdateClassroomLessonCommand(
            classroom.Id, Guid.NewGuid(), Guid.NewGuid(), "x", "y", DifficultyLevel.Beginner
        ), CancellationToken.None);

        await act.Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Fact]
    public async Task UpdateLesson_WithNonExistentLesson_ShouldThrowNotFound()
    {
        var teacher = await CreateUserAsync();
        var classroom = await CreateClassroomAsync(teacher.Id);
        var handler = new UpdateClassroomLessonCommandHandler(_context);

        var act = async () => await handler.Handle(new UpdateClassroomLessonCommand(
            classroom.Id, Guid.NewGuid(), teacher.Id, "x", "y", DifficultyLevel.Beginner
        ), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    // â”€â”€ DeleteClassroomQuiz â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

    [Fact]
    public async Task DeleteQuiz_WithValidData_ShouldRemoveQuiz()
    {
        var teacher = await CreateUserAsync();
        var classroom = await CreateClassroomAsync(teacher.Id);
        var lesson = await CreateLessonAsync(classroom.Id);
        var quiz = await CreateQuizAsync(classroom.Id, lesson.Id);
        var handler = new DeleteClassroomQuizCommandHandler(_context);

        var result = await handler.Handle(
            new DeleteClassroomQuizCommand(classroom.Id, quiz.Id, teacher.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        (await _context.ClassroomQuizzes.CountAsync()).Should().Be(0);
    }

    [Fact]
    public async Task DeleteQuiz_WithNonTeacher_ShouldThrowForbidden()
    {
        var teacher = await CreateUserAsync();
        var classroom = await CreateClassroomAsync(teacher.Id);
        var handler = new DeleteClassroomQuizCommandHandler(_context);

        var act = async () => await handler.Handle(
            new DeleteClassroomQuizCommand(classroom.Id, Guid.NewGuid(), Guid.NewGuid()),
            CancellationToken.None);

        await act.Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Fact]
    public async Task DeleteQuiz_WithNonExistentQuiz_ShouldThrowNotFound()
    {
        var teacher = await CreateUserAsync();
        var classroom = await CreateClassroomAsync(teacher.Id);
        var handler = new DeleteClassroomQuizCommandHandler(_context);

        var act = async () => await handler.Handle(
            new DeleteClassroomQuizCommand(classroom.Id, Guid.NewGuid(), teacher.Id),
            CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    // â”€â”€ UpdateClassroomQuiz â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

    [Fact]
    public async Task UpdateQuiz_WithValidData_ShouldUpdateFields()
    {
        var teacher = await CreateUserAsync();
        var classroom = await CreateClassroomAsync(teacher.Id);
        var lesson = await CreateLessonAsync(classroom.Id);
        var quiz = await CreateQuizAsync(classroom.Id, lesson.Id);
        var handler = new UpdateClassroomQuizCommandHandler(_context);

        var result = await handler.Handle(new UpdateClassroomQuizCommand(
            classroom.Id, quiz.Id, teacher.Id, "Updated", DifficultyLevel.Intermediate, 30
        ), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Title.Should().Be("Updated");
        result.Data.TimeLimitMinutes.Should().Be(30);
    }

    [Fact]
    public async Task UpdateQuiz_WithNonTeacher_ShouldThrowForbidden()
    {
        var teacher = await CreateUserAsync();
        var classroom = await CreateClassroomAsync(teacher.Id);
        var handler = new UpdateClassroomQuizCommandHandler(_context);

        var act = async () => await handler.Handle(new UpdateClassroomQuizCommand(
            classroom.Id, Guid.NewGuid(), Guid.NewGuid(), "x", DifficultyLevel.Beginner, 15
        ), CancellationToken.None);

        await act.Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Fact]
    public async Task UpdateQuiz_WithNonExistentQuiz_ShouldThrowNotFound()
    {
        var teacher = await CreateUserAsync();
        var classroom = await CreateClassroomAsync(teacher.Id);
        var handler = new UpdateClassroomQuizCommandHandler(_context);

        var act = async () => await handler.Handle(new UpdateClassroomQuizCommand(
            classroom.Id, Guid.NewGuid(), teacher.Id, "x", DifficultyLevel.Beginner, 15
        ), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    // â”€â”€ DeleteClassroomQuestion â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

    [Fact]
    public async Task DeleteQuestion_WithValidData_ShouldRemoveQuestion()
    {
        var teacher = await CreateUserAsync();
        var classroom = await CreateClassroomAsync(teacher.Id);
        var lesson = await CreateLessonAsync(classroom.Id);
        var quiz = await CreateQuizAsync(classroom.Id, lesson.Id);
        var question = ClassroomQuestion.Create(quiz.Id, "2+2=?", "4",
            new List<string> { "3", "4", "5" }, 10, "math");
        _context.ClassroomQuestions.Add(question);
        await _context.SaveChangesAsync();

        var handler = new DeleteClassroomQuestionCommandHandler(_context);
        var result = await handler.Handle(
            new DeleteClassroomQuestionCommand(classroom.Id, quiz.Id, question.Id, teacher.Id),
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        (await _context.ClassroomQuestions.CountAsync()).Should().Be(0);
    }

    [Fact]
    public async Task DeleteQuestion_WithNonTeacher_ShouldThrowForbidden()
    {
        var teacher = await CreateUserAsync();
        var classroom = await CreateClassroomAsync(teacher.Id);
        var handler = new DeleteClassroomQuestionCommandHandler(_context);

        var act = async () => await handler.Handle(
            new DeleteClassroomQuestionCommand(classroom.Id, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()),
            CancellationToken.None);

        await act.Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Fact]
    public async Task DeleteQuestion_WithNonExistentQuestion_ShouldThrowNotFound()
    {
        var teacher = await CreateUserAsync();
        var classroom = await CreateClassroomAsync(teacher.Id);
        var lesson = await CreateLessonAsync(classroom.Id);
        var quiz = await CreateQuizAsync(classroom.Id, lesson.Id);
        var handler = new DeleteClassroomQuestionCommandHandler(_context);

        var act = async () => await handler.Handle(
            new DeleteClassroomQuestionCommand(classroom.Id, quiz.Id, Guid.NewGuid(), teacher.Id),
            CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}

