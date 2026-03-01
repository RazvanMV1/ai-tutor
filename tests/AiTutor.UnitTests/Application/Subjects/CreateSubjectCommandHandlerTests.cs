using AiTutor.Application.Features.Subjects.Commands.CreateSubject;
using AiTutor.Domain.Enums;
using FluentAssertions;

namespace AiTutor.UnitTests.Application.Subjects;

public class CreateSubjectCommandHandlerTests
{
    private readonly TestDbContext _context;
    private readonly CreateSubjectCommandHandler _handler;

    public CreateSubjectCommandHandlerTests()
    {
        _context = TestDbContextFactory.Create();
        _handler = new CreateSubjectCommandHandler(_context);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateSubject()
    {
        // Arrange
        var command = new CreateSubjectCommand(
            "Matematicã", "Materie de matematicã", SubjectType.Mathematics);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data!.Name.Should().Be("Matematicã");
        result.Data.Type.Should().Be(SubjectType.Mathematics);
        result.StatusCode.Should().Be(201);
    }

    [Fact]
    public async Task Handle_ShouldPersistSubjectInDatabase()
    {
        // Arrange
        var command = new CreateSubjectCommand(
            "Informaticã", "Materie de informaticã", SubjectType.Informatics);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _context.Subjects.Should().HaveCount(1);
    }
}
