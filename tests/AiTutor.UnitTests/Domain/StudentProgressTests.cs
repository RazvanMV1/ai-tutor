using AiTutor.Domain.Entities;
using AiTutor.Domain.Events;
using FluentAssertions;

namespace AiTutor.UnitTests.Domain;

public class StudentProgressTests
{
    [Fact]
    public void Create_ShouldInitializeWithDefaults()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var lessonId = Guid.NewGuid();

        // Act
        var progress = StudentProgress.Create(userId, lessonId);

        // Assert
        progress.UserId.Should().Be(userId);
        progress.LessonId.Should().Be(lessonId);
        progress.IsCompleted.Should().BeFalse();
        progress.ScorePercentage.Should().Be(0);
        progress.AttemptsCount.Should().Be(0);
    }

    [Fact]
    public void Complete_ShouldSetCompletedAndRaiseEvent()
    {
        // Arrange
        var progress = StudentProgress.Create(Guid.NewGuid(), Guid.NewGuid());

        // Act
        progress.Complete(85);

        // Assert
        progress.IsCompleted.Should().BeTrue();
        progress.ScorePercentage.Should().Be(85);
        progress.AttemptsCount.Should().Be(1);
        progress.CompletedAt.Should().NotBeNull();
        progress.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<LessonCompletedEvent>();
    }

    [Fact]
    public void RegisterAttempt_ShouldIncrementAttemptsCount()
    {
        // Arrange
        var progress = StudentProgress.Create(Guid.NewGuid(), Guid.NewGuid());

        // Act
        progress.RegisterAttempt(60);
        progress.RegisterAttempt(70);

        // Assert
        progress.AttemptsCount.Should().Be(2);
        progress.ScorePercentage.Should().Be(70);
    }
}
