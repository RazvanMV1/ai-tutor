using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Features.Lessons.Commands.CreateLesson;
using AiTutor.Domain.Entities;
using AiTutor.Domain.Enums;
using FluentAssertions;

namespace AiTutor.UnitTests.Application.Lessons;

public class CreateLessonCommandHandlerTests
{
    private readonly TestDbContext _context;
    private readonly CreateLessonCommandHandler _handler;

    public CreateLessonCommandHandlerTests()
    {
        _context = TestDbContextFactory.Create();
        _handler = new CreateLessonCommandHandler(_context);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateLesson()
    {
        // Arrange
        var subject = Subject.Create("Matematică", "Descriere", SubjectType.Mathematics);
        _context.Subjects.Add(subject);
        await _context.SaveChangesAsync(CancellationToken.None);

        var command = new CreateLessonCommand(
            "Ecuații", "Conținut", 1, DifficultyLevel.Beginner, subject.Id);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data!.Title.Should().Be("Ecuații");
        result.Data.SubjectId.Should().Be(subject.Id);
        result.StatusCode.Should().Be(201);
    }

    [Fact]
    public async Task Handle_WithNonExistentSubject_ShouldThrowNotFoundException()
    {
        // Arrange
        var command = new CreateLessonCommand(
            "Ecuații", "Conținut", 1, DifficultyLevel.Beginner, Guid.NewGuid());

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
