using MediatR;

namespace AiTutor.Domain.Events;

public record LessonCompletedEvent(
    Guid UserId,
    Guid LessonId,
    int ScorePercentage) : INotification;
