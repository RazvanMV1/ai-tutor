using AiTutor.Domain.Common;
using AiTutor.Domain.Enums;
using AiTutor.Domain.Exceptions;

namespace AiTutor.Domain.Entities;

public class ClassroomQuiz : BaseEntity
{
    public Guid ClassroomId { get; private set; }
    public Guid ClassroomLessonId { get; private set; }
    public ClassroomLesson Lesson { get; private set; } = null!;
    public string Title { get; private set; } = string.Empty;
    public DifficultyLevel Difficulty { get; private set; }
    public int TimeLimitMinutes { get; private set; }

    private readonly List<ClassroomQuestion> _questions = new();
    public IReadOnlyCollection<ClassroomQuestion> Questions => _questions.AsReadOnly();

    private ClassroomQuiz() { }

    public static ClassroomQuiz Create(Guid classroomId, Guid lessonId, string title,
        DifficultyLevel difficulty, int timeLimitMinutes)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Quiz title cannot be empty.");
        if (timeLimitMinutes <= 0 || timeLimitMinutes > 180)
            throw new DomainException("Time limit must be between 1 and 180 minutes.");

        return new ClassroomQuiz
        {
            ClassroomId = classroomId,
            ClassroomLessonId = lessonId,
            Title = title,
            Difficulty = difficulty,
            TimeLimitMinutes = timeLimitMinutes
        };
    }
}
