using AiTutor.Application.Features.Progress.Commands.CompleteLesson;
using AiTutor.Domain.Entities;
using AiTutor.Domain.Enums;
using FluentAssertions;

namespace AiTutor.UnitTests.Application.Progress;

public class CompleteLessonCommandHandlerTests
{
    private readonly TestDbContext _context;
    private readonly CompleteLessonCommandHandler _handler;

    public CompleteLessonCommandHandlerTests()
    {
        _context = TestDbContextFactory.Create();
        _handler = new CompleteLessonCommandHandler(_context);
    }

    [Fact]
    public async Task Handle_NewProgress_ShouldCreateAndComplete()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var lessonId = Guid.NewGuid();
        var command = new CompleteLessonCommand(userId, lessonId, 90);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data!.IsCompleted.Should().BeTrue();
        result.Data.ScorePercentage.Should().Be(90);
        result.Data.CompletedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_ExistingProgress_ShouldUpdateAndComplete()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var lessonId = Guid.NewGuid();

        var existingProgress = StudentProgress.Create(userId, lessonId);
        _context.StudentProgresses.Add(existingProgress);
        await _context.SaveChangesAsync(CancellationToken.None);

        var command = new CompleteLessonCommand(userId, lessonId, 75);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data!.IsCompleted.Should().BeTrue();
        result.Data.ScorePercentage.Should().Be(75);
        _context.StudentProgresses.Should().HaveCount(1);
    }
}
