using AiTutor.Domain.Common;
using AiTutor.Domain.Events;

namespace AiTutor.Domain.Entities;

public class StudentProgress : BaseEntity
{
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;
    public Guid LessonId { get; private set; }
    public Lesson Lesson { get; private set; } = null!;
    public bool IsCompleted { get; private set; }
    public int ScorePercentage { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public int AttemptsCount { get; private set; }

    private StudentProgress() { }

    public static StudentProgress Create(Guid userId, Guid lessonId)
    {
        return new StudentProgress
        {
            UserId = userId,
            LessonId = lessonId,
            IsCompleted = false,
            ScorePercentage = 0,
            AttemptsCount = 0
        };
    }

    public void Complete(int scorePercentage)
    {
        IsCompleted = true;
        ScorePercentage = scorePercentage;
        CompletedAt = DateTime.UtcNow;
        AttemptsCount++;
        SetUpdatedAt();
        AddDomainEvent(new LessonCompletedEvent(UserId, LessonId, scorePercentage));
    }

    public void RegisterAttempt(int scorePercentage)
    {
        AttemptsCount++;
        ScorePercentage = scorePercentage;
        SetUpdatedAt();
    }
}
