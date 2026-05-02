using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Features.Classrooms.Queries.GetClassroomLessonById;
using AiTutor.Application.Features.Classrooms.Queries.GetClassroomQuizById;
using AiTutor.Application.Features.Classrooms.Queries.GetClassroomQuizzesByLesson;
using AiTutor.Domain.Entities;
using AiTutor.Domain.Enums;
using AiTutor.UnitTests.Application;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AiTutor.UnitTests.Application.Classrooms;

public class ClassroomQueryByIdTests : IDisposable
{
    private readonly TestDbContext _context;

    public ClassroomQueryByIdTests()
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

    // â”€â”€ GetClassroomLessonById â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

    [Fact]
    public async Task GetLessonById_AsTeacher_ShouldReturnLesson()
    {
        var teacher = await CreateUserAsync();
        var classroom = await CreateClassroomAsync(teacher.Id);
        var lesson = ClassroomLesson.Create(classroom.Id, "L", "c", 0, DifficultyLevel.Beginner);
        _context.ClassroomLessons.Add(lesson);
        await _context.SaveChangesAsync();

        var handler = new GetClassroomLessonByIdQueryHandler(_context);
        var result = await handler.Handle(
            new GetClassroomLessonByIdQuery(classroom.Id, lesson.Id, teacher.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Id.Should().Be(lesson.Id);
        result.Data.Title.Should().Be("L");
    }

    [Fact]
    public async Task GetLessonById_AsStudent_ShouldReturnLesson()
    {
        var teacher = await CreateUserAsync();
        var student = await CreateUserAsync(UserRole.Student);
        var classroom = await CreateClassroomAsync(teacher.Id);
        var member = ClassroomMember.Create(classroom.Id, student.Id);
        _context.ClassroomMembers.Add(member);
        var lesson = ClassroomLesson.Create(classroom.Id, "L", "c", 0, DifficultyLevel.Beginner);
        _context.ClassroomLessons.Add(lesson);
        await _context.SaveChangesAsync();

        var handler = new GetClassroomLessonByIdQueryHandler(_context);
        var result = await handler.Handle(
            new GetClassroomLessonByIdQuery(classroom.Id, lesson.Id, student.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task GetLessonById_AsNonMember_ShouldThrowForbidden()
    {
        var teacher = await CreateUserAsync();
        var classroom = await CreateClassroomAsync(teacher.Id);
        var handler = new GetClassroomLessonByIdQueryHandler(_context);

        var act = async () => await handler.Handle(
            new GetClassroomLessonByIdQuery(classroom.Id, Guid.NewGuid(), Guid.NewGuid()),
            CancellationToken.None);

        await act.Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Fact]
    public async Task GetLessonById_WithNonExistentClassroom_ShouldThrowNotFound()
    {
        var handler = new GetClassroomLessonByIdQueryHandler(_context);
        var act = async () => await handler.Handle(
            new GetClassroomLessonByIdQuery(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()),
            CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task GetLessonById_WithNonExistentLesson_ShouldThrowNotFound()
    {
        var teacher = await CreateUserAsync();
        var classroom = await CreateClassroomAsync(teacher.Id);
        var handler = new GetClassroomLessonByIdQueryHandler(_context);

        var act = async () => await handler.Handle(
            new GetClassroomLessonByIdQuery(classroom.Id, Guid.NewGuid(), teacher.Id),
            CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    // â”€â”€ GetClassroomQuizById â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

    [Fact]
    public async Task GetQuizById_AsTeacher_ShouldReturnQuizWithQuestions()
    {
        var teacher = await CreateUserAsync();
        var classroom = await CreateClassroomAsync(teacher.Id);
        var lesson = ClassroomLesson.Create(classroom.Id, "L", "c", 0, DifficultyLevel.Beginner);
        _context.ClassroomLessons.Add(lesson);
        var quiz = ClassroomQuiz.Create(classroom.Id, lesson.Id, "Q", DifficultyLevel.Beginner, 15);
        _context.ClassroomQuizzes.Add(quiz);
        var question = ClassroomQuestion.Create(quiz.Id, "q?", "a",
            new List<string> { "a", "b" }, 10);
        _context.ClassroomQuestions.Add(question);
        await _context.SaveChangesAsync();

        var handler = new GetClassroomQuizByIdQueryHandler(_context);
        var result = await handler.Handle(
            new GetClassroomQuizByIdQuery(classroom.Id, quiz.Id, teacher.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Id.Should().Be(quiz.Id);
        result.Data.Questions.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetQuizById_AsNonMember_ShouldThrowForbidden()
    {
        var teacher = await CreateUserAsync();
        var classroom = await CreateClassroomAsync(teacher.Id);
        var handler = new GetClassroomQuizByIdQueryHandler(_context);

        var act = async () => await handler.Handle(
            new GetClassroomQuizByIdQuery(classroom.Id, Guid.NewGuid(), Guid.NewGuid()),
            CancellationToken.None);

        await act.Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Fact]
    public async Task GetQuizById_WithNonExistentQuiz_ShouldThrowNotFound()
    {
        var teacher = await CreateUserAsync();
        var classroom = await CreateClassroomAsync(teacher.Id);
        var handler = new GetClassroomQuizByIdQueryHandler(_context);

        var act = async () => await handler.Handle(
            new GetClassroomQuizByIdQuery(classroom.Id, Guid.NewGuid(), teacher.Id),
            CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task GetQuizById_WithNonExistentClassroom_ShouldThrowNotFound()
    {
        var handler = new GetClassroomQuizByIdQueryHandler(_context);
        var act = async () => await handler.Handle(
            new GetClassroomQuizByIdQuery(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()),
            CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    // â”€â”€ GetClassroomQuizzesByLesson â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

    [Fact]
    public async Task GetQuizzesByLesson_AsTeacher_ShouldReturnList()
    {
        var teacher = await CreateUserAsync();
        var classroom = await CreateClassroomAsync(teacher.Id);
        var lesson = ClassroomLesson.Create(classroom.Id, "L", "c", 0, DifficultyLevel.Beginner);
        _context.ClassroomLessons.Add(lesson);
        var quiz = ClassroomQuiz.Create(classroom.Id, lesson.Id, "Q", DifficultyLevel.Beginner, 15);
        _context.ClassroomQuizzes.Add(quiz);
        await _context.SaveChangesAsync();

        var handler = new GetClassroomQuizzesByLessonQueryHandler(_context);
        var result = await handler.Handle(
            new GetClassroomQuizzesByLessonQuery(classroom.Id, lesson.Id, teacher.Id),
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetQuizzesByLesson_AsNonMember_ShouldThrowForbidden()
    {
        var teacher = await CreateUserAsync();
        var classroom = await CreateClassroomAsync(teacher.Id);
        var handler = new GetClassroomQuizzesByLessonQueryHandler(_context);

        var act = async () => await handler.Handle(
            new GetClassroomQuizzesByLessonQuery(classroom.Id, Guid.NewGuid(), Guid.NewGuid()),
            CancellationToken.None);

        await act.Should().ThrowAsync<ForbiddenAccessException>();
    }

    [Fact]
    public async Task GetQuizzesByLesson_WithNonExistentClassroom_ShouldThrowNotFound()
    {
        var handler = new GetClassroomQuizzesByLessonQueryHandler(_context);
        var act = async () => await handler.Handle(
            new GetClassroomQuizzesByLessonQuery(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()),
            CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}

