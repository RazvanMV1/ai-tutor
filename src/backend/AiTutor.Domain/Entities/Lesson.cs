using AiTutor.Domain.Common;
using AiTutor.Domain.Enums;

namespace AiTutor.Domain.Entities;

public class Lesson : BaseEntity
{
    public string Title { get; private set; } = string.Empty;
    public string Content { get; private set; } = string.Empty;
    public int OrderIndex { get; private set; }
    public DifficultyLevel Difficulty { get; private set; }
    public Guid SubjectId { get; private set; }
    public Subject Subject { get; private set; } = null!;

    private readonly List<Quiz> _quizzes = new();
    public IReadOnlyCollection<Quiz> Quizzes => _quizzes.AsReadOnly();

    private Lesson() { }

    public static Lesson Create(string title, string content, int orderIndex,
        DifficultyLevel difficulty, Guid subjectId)
    {
        return new Lesson
        {
            Title = title,
            Content = content,
            OrderIndex = orderIndex,
            Difficulty = difficulty,
            SubjectId = subjectId
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
