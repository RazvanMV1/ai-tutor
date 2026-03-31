using AiTutor.Application.Features.Classrooms.Commands.AddClassroomQuestion;
using AiTutor.Application.Features.Classrooms.Commands.AddMember;
using AiTutor.Application.Features.Classrooms.Commands.CreateClassroomLesson;
using AiTutor.Application.Features.Classrooms.Commands.CreateClassroomQuiz;
using AiTutor.Application.Features.Classrooms.Commands.DeleteClassroom;
using AiTutor.Application.Features.Classrooms.Commands.DeleteGrade;
using AiTutor.Application.Features.Classrooms.Commands.RemoveMember;
using AiTutor.Application.Features.Classrooms.Commands.SubmitClassroomQuiz;
using AiTutor.Application.Features.Classrooms.Commands.UpdateClassroom;
using AiTutor.Application.Features.Classrooms.Commands.UpdateGrade;
using AiTutor.Application.Features.Classrooms.Queries.GetClassroomById;
using AiTutor.Application.Features.Classrooms.Queries.GetClassroomGrades;
using AiTutor.Application.Features.Classrooms.Queries.GetClassroomLessons;
using AiTutor.Application.Features.Classrooms.Queries.GetClassroomMembers;
using AiTutor.Application.Features.Classrooms.Queries.GetClassroomProgress;
using AiTutor.Application.Features.Classrooms.Queries.GetMyClassrooms;
using AiTutor.Domain.Entities;
using AiTutor.Domain.Enums;
using AiTutor.UnitTests.Application;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AiTutor.UnitTests.Application.Classrooms;

public class ClassroomExtendedTests : IDisposable
{
    private readonly TestDbContext _context;

    public ClassroomExtendedTests()
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

    private async Task<Classroom> CreateClassroomAsync(Guid teacherId)
    {
        var classroom = Classroom.Create("Test Classroom", "Description",
            SubjectType.Mathematics, teacherId);
        _context.Classrooms.Add(classroom);
        await _context.SaveChangesAsync();
        return classroom;
    }

    private async Task<ClassroomMember> AddMemberAsync(Guid classroomId, Guid studentId)
    {
        var member = ClassroomMember.Create(classroomId, studentId);
        _context.ClassroomMembers.Add(member);
        await _context.SaveChangesAsync();
        return member;
    }

    private async Task<ClassroomLesson> AddLessonAsync(Guid classroomId)
    {
        var lesson = ClassroomLesson.Create(classroomId, "Test Lesson", "Content", 1, DifficultyLevel.Beginner);
        _context.ClassroomLessons.Add(lesson);
        await _context.SaveChangesAsync();
        return lesson;
    }

    private async Task<ClassroomQuiz> AddQuizAsync(Guid classroomId, Guid lessonId)
    {
        var quiz = ClassroomQuiz.Create(classroomId, lessonId, "Test Quiz", DifficultyLevel.Beginner, 30);
        _context.ClassroomQuizzes.Add(quiz);
        await _context.SaveChangesAsync();
        return quiz;
    }

    // ── AddMember ──────────────────────────────────────────────────────
    [Fact]
    public async Task AddMember_WithValidData_ShouldSucceed()
    {
        var teacher = await CreateUserAsync(UserRole.Teacher);
        var student = await CreateUserAsync(UserRole.Student);
        var classroom = await CreateClassroomAsync(teacher.Id);
        var handler = new AddMemberCommandHandler(_context);
        var command = new AddMemberCommand(classroom.Id, teacher.Id, student.Email.Value);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task AddMember_WithNonTeacher_ShouldThrowForbiddenAccessException()
    {
        var teacher = await CreateUserAsync(UserRole.Teacher);
        var student = await CreateUserAsync(UserRole.Student);
        var fakeTeacher = await CreateUserAsync(UserRole.Student);
        var classroom = await CreateClassroomAsync(teacher.Id);
        var handler = new AddMemberCommandHandler(_context);
        var command = new AddMemberCommand(classroom.Id, fakeTeacher.Id, student.Email.Value);

        var act = async () => await handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<AiTutor.Application.Common.Exceptions.ForbiddenAccessException>();
    }

    // ── RemoveMember ───────────────────────────────────────────────────
    [Fact]
    public async Task RemoveMember_WithValidData_ShouldSucceed()
    {
        var teacher = await CreateUserAsync(UserRole.Teacher);
        var student = await CreateUserAsync(UserRole.Student);
        var classroom = await CreateClassroomAsync(teacher.Id);
        await AddMemberAsync(classroom.Id, student.Id);
        var handler = new RemoveMemberCommandHandler(_context);
        var command = new RemoveMemberCommand(classroom.Id, teacher.Id, student.Id);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task RemoveMember_WithNonTeacher_ShouldThrowForbiddenAccessException()
    {
        var teacher = await CreateUserAsync(UserRole.Teacher);
        var student = await CreateUserAsync(UserRole.Student);
        var fakeUser = await CreateUserAsync(UserRole.Student);
        var classroom = await CreateClassroomAsync(teacher.Id);
        await AddMemberAsync(classroom.Id, student.Id);
        var handler = new RemoveMemberCommandHandler(_context);
        var command = new RemoveMemberCommand(classroom.Id, fakeUser.Id, student.Id);

        var act = async () => await handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<AiTutor.Application.Common.Exceptions.ForbiddenAccessException>();
    }

    // ── DeleteClassroom ────────────────────────────────────────────────
    [Fact]
    public async Task DeleteClassroom_WithValidData_ShouldSucceed()
    {
        var teacher = await CreateUserAsync(UserRole.Teacher);
        var classroom = await CreateClassroomAsync(teacher.Id);
        var handler = new DeleteClassroomCommandHandler(_context);
        var command = new DeleteClassroomCommand(classroom.Id, teacher.Id);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteClassroom_WithNonTeacher_ShouldThrowForbiddenAccessException()
    {
        var teacher = await CreateUserAsync(UserRole.Teacher);
        var fakeTeacher = await CreateUserAsync(UserRole.Student);
        var classroom = await CreateClassroomAsync(teacher.Id);
        var handler = new DeleteClassroomCommandHandler(_context);
        var command = new DeleteClassroomCommand(classroom.Id, fakeTeacher.Id);

        var act = async () => await handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<AiTutor.Application.Common.Exceptions.ForbiddenAccessException>();
    }

    // ── UpdateClassroom ────────────────────────────────────────────────
    [Fact]
    public async Task UpdateClassroom_WithValidData_ShouldSucceed()
    {
        var teacher = await CreateUserAsync(UserRole.Teacher);
        var classroom = await CreateClassroomAsync(teacher.Id);
        var handler = new UpdateClassroomCommandHandler(_context);
        var command = new UpdateClassroomCommand(classroom.Id, teacher.Id, "New Name", "New Description");

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Name.Should().Be("New Name");
    }

    // ── CreateClassroomLesson ──────────────────────────────────────────
    [Fact]
    public async Task CreateClassroomLesson_WithValidData_ShouldSucceed()
    {
        var teacher = await CreateUserAsync(UserRole.Teacher);
        var classroom = await CreateClassroomAsync(teacher.Id);
        var handler = new CreateClassroomLessonCommandHandler(_context);
        var command = new CreateClassroomLessonCommand(
            classroom.Id, teacher.Id, "Lectie noua", "Continut", 1, DifficultyLevel.Beginner);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Title.Should().Be("Lectie noua");
    }

    [Fact]
    public async Task CreateClassroomLesson_WithNonTeacher_ShouldThrowForbiddenAccessException()
    {
        var teacher = await CreateUserAsync(UserRole.Teacher);
        var fakeTeacher = await CreateUserAsync(UserRole.Student);
        var classroom = await CreateClassroomAsync(teacher.Id);
        var handler = new CreateClassroomLessonCommandHandler(_context);
        var command = new CreateClassroomLessonCommand(
            classroom.Id, fakeTeacher.Id, "Lectie", "Continut", 1, DifficultyLevel.Beginner);

        var act = async () => await handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<AiTutor.Application.Common.Exceptions.ForbiddenAccessException>();
    }

    // ── CreateClassroomQuiz ────────────────────────────────────────────
    [Fact]
    public async Task CreateClassroomQuiz_WithValidData_ShouldSucceed()
    {
        var teacher = await CreateUserAsync(UserRole.Teacher);
        var classroom = await CreateClassroomAsync(teacher.Id);
        var lesson = await AddLessonAsync(classroom.Id);
        var handler = new CreateClassroomQuizCommandHandler(_context);
        var command = new CreateClassroomQuizCommand(
            classroom.Id, lesson.Id, teacher.Id, "Quiz nou", DifficultyLevel.Beginner, 30);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Title.Should().Be("Quiz nou");
    }

    // ── AddClassroomQuestion ───────────────────────────────────────────
    [Fact]
    public async Task AddClassroomQuestion_WithValidData_ShouldSucceed()
    {
        var teacher = await CreateUserAsync(UserRole.Teacher);
        var classroom = await CreateClassroomAsync(teacher.Id);
        var lesson = await AddLessonAsync(classroom.Id);

        // Creează quiz direct folosind CreateClassroomQuizCommandHandler
        // ca să fie salvat corect în context
        var quizHandler = new CreateClassroomQuizCommandHandler(_context);
        var quizCommand = new CreateClassroomQuizCommand(
            classroom.Id, lesson.Id, teacher.Id, "Test Quiz", DifficultyLevel.Beginner, 30);
        var quizResult = await quizHandler.Handle(quizCommand, CancellationToken.None);
        quizResult.IsSuccess.Should().BeTrue();
        var quizId = quizResult.Data!.Id;

        var handler = new AddClassroomQuestionCommandHandler(_context);
        var command = new AddClassroomQuestionCommand(
            quizId, classroom.Id, teacher.Id,
            "Care este 2+2?", "4",
            new List<string> { "2", "3", "4", "5" }, 10, "Matematica de baza");

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Text.Should().Be("Care este 2+2?");
    }

    // ── SubmitClassroomQuiz ────────────────────────────────────────────
    [Fact]
    public async Task SubmitClassroomQuiz_WithValidData_ShouldSucceed()
    {
        var teacher = await CreateUserAsync(UserRole.Teacher);
        var student = await CreateUserAsync(UserRole.Student);
        var classroom = await CreateClassroomAsync(teacher.Id);
        await AddMemberAsync(classroom.Id, student.Id);
        var lesson = await AddLessonAsync(classroom.Id);
        var quiz = await AddQuizAsync(classroom.Id, lesson.Id);
        var question = ClassroomQuestion.Create(quiz.Id, "Care este 2+2?", "4",
            new List<string> { "2", "3", "4", "5" }, 10);
        _context.ClassroomQuestions.Add(question);
        await _context.SaveChangesAsync();

        var handler = new SubmitClassroomQuizCommandHandler(_context);
        var command = new SubmitClassroomQuizCommand(
            classroom.Id, quiz.Id, student.Id,
            new List<QuizAnswerDto> { new QuizAnswerDto(question.Id, "4") });

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
    }

    // ── UpdateGrade ────────────────────────────────────────────────────
    [Fact]
    public async Task UpdateGrade_WithValidData_ShouldSucceed()
    {
        var teacher = await CreateUserAsync(UserRole.Teacher);
        var student = await CreateUserAsync(UserRole.Student);
        var classroom = await CreateClassroomAsync(teacher.Id);
        await AddMemberAsync(classroom.Id, student.Id);
        var grade = Grade.Create(classroom.Id, student.Id, teacher.Id, 8, "Initial");
        _context.Grades.Add(grade);
        await _context.SaveChangesAsync();
        var handler = new UpdateGradeCommandHandler(_context);
        var command = new UpdateGradeCommand(grade.Id, teacher.Id, 9, "Updated");

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Value.Should().Be(9);
    }

    // ── DeleteGrade ────────────────────────────────────────────────────
    [Fact]
    public async Task DeleteGrade_WithValidData_ShouldSucceed()
    {
        var teacher = await CreateUserAsync(UserRole.Teacher);
        var student = await CreateUserAsync(UserRole.Student);
        var classroom = await CreateClassroomAsync(teacher.Id);
        var grade = Grade.Create(classroom.Id, student.Id, teacher.Id, 8, "Test");
        _context.Grades.Add(grade);
        await _context.SaveChangesAsync();
        var handler = new DeleteGradeCommandHandler(_context);
        var command = new DeleteGradeCommand(grade.Id, teacher.Id);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
    }

    // ── GetMyClassrooms ────────────────────────────────────────────────
    [Fact]
    public async Task GetMyClassrooms_AsTeacher_ShouldReturnClassrooms()
    {
        var teacher = await CreateUserAsync(UserRole.Teacher);
        await CreateClassroomAsync(teacher.Id);
        await CreateClassroomAsync(teacher.Id);
        var handler = new GetMyClassroomsQueryHandler(_context);
        var query = new GetMyClassroomsQuery(teacher.Id);

        var result = await handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Count.Should().Be(2);
    }

    // ── GetClassroomById ───────────────────────────────────────────────
    [Fact]
    public async Task GetClassroomById_WithValidId_ShouldReturnClassroom()
    {
        var teacher = await CreateUserAsync(UserRole.Teacher);
        var classroom = await CreateClassroomAsync(teacher.Id);
        var handler = new GetClassroomByIdQueryHandler(_context);
        var query = new GetClassroomByIdQuery(classroom.Id, teacher.Id);

        var result = await handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Name.Should().Be("Test Classroom");
    }

    [Fact]
    public async Task GetClassroomById_WithInvalidId_ShouldThrowNotFoundException()
    {
        var teacher = await CreateUserAsync(UserRole.Teacher);
        var handler = new GetClassroomByIdQueryHandler(_context);
        var query = new GetClassroomByIdQuery(Guid.NewGuid(), teacher.Id);

        var act = async () => await handler.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<AiTutor.Application.Common.Exceptions.NotFoundException>();
    }

    // ── GetClassroomMembers ────────────────────────────────────────────
    [Fact]
    public async Task GetClassroomMembers_ShouldReturnMembers()
    {
        var teacher = await CreateUserAsync(UserRole.Teacher);
        var student = await CreateUserAsync(UserRole.Student);
        var classroom = await CreateClassroomAsync(teacher.Id);
        await AddMemberAsync(classroom.Id, student.Id);
        var handler = new GetClassroomMembersQueryHandler(_context);
        var query = new GetClassroomMembersQuery(classroom.Id, teacher.Id);

        var result = await handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Count.Should().Be(1);
    }

    // ── GetClassroomLessons ────────────────────────────────────────────
    [Fact]
    public async Task GetClassroomLessons_ShouldReturnLessons()
    {
        var teacher = await CreateUserAsync(UserRole.Teacher);
        var classroom = await CreateClassroomAsync(teacher.Id);
        await AddLessonAsync(classroom.Id);
        await AddLessonAsync(classroom.Id);
        var handler = new GetClassroomLessonsQueryHandler(_context);
        var query = new GetClassroomLessonsQuery(classroom.Id, teacher.Id);

        var result = await handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Count.Should().Be(2);
    }

    // ── GetClassroomGrades ─────────────────────────────────────────────
    [Fact]
    public async Task GetClassroomGrades_AsTeacher_ShouldReturnAllGrades()
    {
        var teacher = await CreateUserAsync(UserRole.Teacher);
        var student = await CreateUserAsync(UserRole.Student);
        var classroom = await CreateClassroomAsync(teacher.Id);
        var grade = Grade.Create(classroom.Id, student.Id, teacher.Id, 9, "Test");
        _context.Grades.Add(grade);
        await _context.SaveChangesAsync();
        var handler = new GetClassroomGradesQueryHandler(_context);
        var query = new GetClassroomGradesQuery(classroom.Id, teacher.Id, true);

        var result = await handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Count.Should().Be(1);
    }

    // ── GetClassroomProgress ───────────────────────────────────────────
    [Fact]
    public async Task GetClassroomProgress_ShouldReturnProgress()
    {
        var teacher = await CreateUserAsync(UserRole.Teacher);
        var student = await CreateUserAsync(UserRole.Student);
        var classroom = await CreateClassroomAsync(teacher.Id);
        await AddMemberAsync(classroom.Id, student.Id);
        var handler = new GetClassroomProgressQueryHandler(_context);
        var query = new GetClassroomProgressQuery(classroom.Id, teacher.Id);

        var result = await handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
    }
}
