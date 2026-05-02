using AiTutor.Application.Features.Users.Queries.GetStudentGrades;
using AiTutor.Domain.Entities;
using AiTutor.Domain.Enums;
using AiTutor.UnitTests.Application;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AiTutor.UnitTests.Application.Users;

public class GetStudentGradesTests : IDisposable
{
    private readonly TestDbContext _context;

    public GetStudentGradesTests()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new TestDbContext(options);
    }

    public void Dispose() => _context.Dispose();

    private async Task<User> CreateUserAsync(UserRole role)
    {
        var u = User.Create("First", "Last", $"u{Guid.NewGuid():N}@t.com", role);
        _context.Users.Add(u);
        await _context.SaveChangesAsync();
        return u;
    }

    [Fact]
    public async Task GetStudentGrades_AsStudentSelf_ShouldReturnGrades()
    {
        var teacher = await CreateUserAsync(UserRole.Teacher);
        var student = await CreateUserAsync(UserRole.Student);
        var classroom = Classroom.Create("Math", "d", SubjectType.Mathematics, teacher.Id);
        _context.Classrooms.Add(classroom);
        var grade = Grade.Create(classroom.Id, student.Id, teacher.Id, 9, "Excellent");
        _context.Grades.Add(grade);
        await _context.SaveChangesAsync();

        var handler = new GetStudentGradesQueryHandler(_context);
        var result = await handler.Handle(
            new GetStudentGradesQuery(student.Id, student.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Should().HaveCount(1);
        result.Data[0].Value.Should().Be(9);
        result.Data[0].SubjectName.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetStudentGrades_AsParent_ShouldReturnGrades()
    {
        var teacher = await CreateUserAsync(UserRole.Teacher);
        var parent = await CreateUserAsync(UserRole.Parent);
        var student = await CreateUserAsync(UserRole.Student);
        student.LinkToParent(parent.Id);
        var classroom = Classroom.Create("Romana", "d", SubjectType.Romanian, teacher.Id);
        _context.Classrooms.Add(classroom);
        _context.Grades.Add(Grade.Create(classroom.Id, student.Id, teacher.Id, 8, "Good"));
        _context.Grades.Add(Grade.Create(classroom.Id, student.Id, teacher.Id, 10, "Perfect"));
        await _context.SaveChangesAsync();

        var handler = new GetStudentGradesQueryHandler(_context);
        var result = await handler.Handle(
            new GetStudentGradesQuery(student.Id, parent.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetStudentGrades_AsUnrelatedUser_ShouldReturnForbidden()
    {
        var student = await CreateUserAsync(UserRole.Student);
        var stranger = await CreateUserAsync(UserRole.Teacher);
        var handler = new GetStudentGradesQueryHandler(_context);

        var result = await handler.Handle(
            new GetStudentGradesQuery(student.Id, stranger.Id), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(403);
    }

    [Fact]
    public async Task GetStudentGrades_StudentNotFound_ShouldReturnNotFound()
    {
        var handler = new GetStudentGradesQueryHandler(_context);
        var result = await handler.Handle(
            new GetStudentGradesQuery(Guid.NewGuid(), Guid.NewGuid()), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task GetStudentGrades_NoGrades_ShouldReturnEmpty()
    {
        var student = await CreateUserAsync(UserRole.Student);
        var handler = new GetStudentGradesQueryHandler(_context);

        var result = await handler.Handle(
            new GetStudentGradesQuery(student.Id, student.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Should().BeEmpty();
    }

    [Theory]
    [InlineData(SubjectType.Mathematics)]
    [InlineData(SubjectType.Romanian)]
    [InlineData(SubjectType.Informatics)]
    public async Task GetStudentGrades_ShouldMapSubjectName(SubjectType subject)
    {
        var teacher = await CreateUserAsync(UserRole.Teacher);
        var student = await CreateUserAsync(UserRole.Student);
        var classroom = Classroom.Create("X", "d", subject, teacher.Id);
        _context.Classrooms.Add(classroom);
        _context.Grades.Add(Grade.Create(classroom.Id, student.Id, teacher.Id, 7, "ok"));
        await _context.SaveChangesAsync();

        var handler = new GetStudentGradesQueryHandler(_context);
        var result = await handler.Handle(
            new GetStudentGradesQuery(student.Id, student.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data![0].SubjectName.Should().NotBeNullOrEmpty();
    }
}
