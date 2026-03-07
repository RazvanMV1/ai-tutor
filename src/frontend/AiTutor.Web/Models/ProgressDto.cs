namespace AiTutor.Web.Models;

public record StudentProgressDto(
    Guid LessonId,
    string LessonTitle,
    bool IsCompleted,
    int ScorePercentage,
    int AttemptsCount,
    DateTime? CompletedAt);

public record CompleteLessonRequest(
    Guid UserId,
    Guid LessonId,
    int ScorePercentage);
