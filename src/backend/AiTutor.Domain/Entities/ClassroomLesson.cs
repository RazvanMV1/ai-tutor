using AiTutor.Domain.Common;
using AiTutor.Domain.Enums;
using AiTutor.Domain.Exceptions;

namespace AiTutor.Domain.Entities;

public class ClassroomLesson : BaseEntity
{
    public Guid ClassroomId { get; private set; }
    public Classroom Classroom { get; private set; } = null!;
    public string Title { get; private set; } = string.Empty;
    public string Content { get; private set; } = string.Empty;
    public int OrderIndex { get; private set; }
    public DifficultyLevel Difficulty { get; private set; }

    private readonly List<ClassroomQuiz> _quizzes = new();
    public IReadOnlyCollection<ClassroomQuiz> Quizzes => _quizzes.AsReadOnly();

    private ClassroomLesson() { }

    public static ClassroomLesson Create(Guid classroomId, string title, string content,
        int orderIndex, DifficultyLevel difficulty)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Lesson title cannot be empty.");

        return new ClassroomLesson
        {
            ClassroomId = classroomId,
            Title = title,
            Content = content,
            OrderIndex = orderIndex,
            Difficulty = difficulty
        };
    }

    public void Update(string title, string content, DifficultyLevel difficulty)
    {
        Title = title;
        Content = content;
        Difficulty = difficulty;
        SetUpdatedAt();
    }
}
