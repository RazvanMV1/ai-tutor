namespace AiTutor.Web.Models;

public record StudentProgressDto(
    Guid Id,
    Guid UserId,
    Guid LessonId,
    bool IsCompleted,
    int ScorePercentage,
    int AttemptsCount,
    DateTime? CompletedAt)
{
    public string LessonTitle { get; init; } = "Lecție";
}

public record CompleteLessonRequest(
    Guid UserId,
    Guid LessonId,
    int ScorePercentage);
