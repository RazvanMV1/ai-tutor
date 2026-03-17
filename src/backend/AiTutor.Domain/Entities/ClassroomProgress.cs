using AiTutor.Domain.Common;

namespace AiTutor.Domain.Entities;

public class ClassroomProgress : BaseEntity
{
    public Guid ClassroomId { get; private set; }
    public Guid StudentId { get; private set; }
    public User Student { get; private set; } = null!;
    public Guid ClassroomLessonId { get; private set; }
    public ClassroomLesson Lesson { get; private set; } = null!;
    public bool IsCompleted { get; private set; }
    public int ScorePercentage { get; private set; }
    public int AttemptsCount { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    private ClassroomProgress() { }

    public static ClassroomProgress Create(Guid classroomId, Guid studentId, Guid lessonId)
    {
        return new ClassroomProgress
        {
            ClassroomId = classroomId,
            StudentId = studentId,
            ClassroomLessonId = lessonId,
            IsCompleted = false,
            ScorePercentage = 0,
            AttemptsCount = 0
        };
    }

    public void Complete(int score)
    {
        IsCompleted = true;
        ScorePercentage = score;
        AttemptsCount++;
        CompletedAt = DateTime.UtcNow;
        SetUpdatedAt();
    }

    public void RegisterAttempt(int score)
    {
        AttemptsCount++;
        ScorePercentage = score;
        if (!IsCompleted) IsCompleted = true;
        CompletedAt = DateTime.UtcNow;
        SetUpdatedAt();
    }
}
