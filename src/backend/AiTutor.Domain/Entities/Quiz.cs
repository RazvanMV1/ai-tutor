using AiTutor.Domain.Common;
using AiTutor.Domain.Enums;
using AiTutor.Domain.Exceptions;

namespace AiTutor.Domain.Entities;

public class Quiz : BaseEntity
{
    public string Title { get; private set; } = string.Empty;
    public DifficultyLevel Difficulty { get; private set; }
    public Guid LessonId { get; private set; }
    public Lesson Lesson { get; private set; } = null!;
    public int TimeLimitMinutes { get; private set; }

    private readonly List<Question> _questions = new();
    public IReadOnlyCollection<Question> Questions => _questions.AsReadOnly();

    private Quiz() { }

    public static Quiz Create(string title, DifficultyLevel difficulty,
        Guid lessonId, int timeLimitMinutes = 30)
    {
        if (timeLimitMinutes <= 0)
            throw new DomainException("Time limit must be greater than 0.");

        return new Quiz
        {
            Title = title,
            Difficulty = difficulty,
            LessonId = lessonId,
            TimeLimitMinutes = timeLimitMinutes
        };
    }

    public void AddQuestion(Question question) =>
        _questions.Append(question);

    public void Update(string title, DifficultyLevel difficulty, int timeLimitMinutes)
    {
        Title = title;
        Difficulty = difficulty;
        TimeLimitMinutes = timeLimitMinutes;
        SetUpdatedAt();
    }
}
