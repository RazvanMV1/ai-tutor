using AiTutor.Domain.Entities;
using AiTutor.Domain.Enums;
using FluentAssertions;

namespace AiTutor.UnitTests.Domain;

public class LessonTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateLesson()
    {
        // Arrange
        var subjectId = Guid.NewGuid();

        // Act
        var lesson = Lesson.Create(
            "Ecuații de gradul 2",
            "Conținut lecție",
            1,
            DifficultyLevel.Beginner,
            subjectId);

        // Assert
        lesson.Should().NotBeNull();
        lesson.Title.Should().Be("Ecuații de gradul 2");
        lesson.OrderIndex.Should().Be(1);
        lesson.Difficulty.Should().Be(DifficultyLevel.Beginner);
        lesson.SubjectId.Should().Be(subjectId);
    }

    [Fact]
    public void Update_ShouldUpdateFieldsAndSetUpdatedAt()
    {
        // Arrange
        var lesson = Lesson.Create("Title", "Content", 1,
            DifficultyLevel.Beginner, Guid.NewGuid());

        // Act
        lesson.Update("New Title", "New Content", DifficultyLevel.Advanced);

        // Assert
        lesson.Title.Should().Be("New Title");
        lesson.Content.Should().Be("New Content");
        lesson.Difficulty.Should().Be(DifficultyLevel.Advanced);
        lesson.UpdatedAt.Should().NotBeNull();
    }
}
