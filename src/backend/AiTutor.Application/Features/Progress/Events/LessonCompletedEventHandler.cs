using AiTutor.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AiTutor.Application.Features.Progress.Events;

public class LessonCompletedEventHandler : INotificationHandler<LessonCompletedEvent>
{
    private readonly ILogger<LessonCompletedEventHandler> _logger;

    public LessonCompletedEventHandler(ILogger<LessonCompletedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(LessonCompletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Lesson {LessonId} completed by user {UserId} with score {Score}%",
            notification.LessonId,
            notification.UserId,
            notification.ScorePercentage);

        return Task.CompletedTask;
    }
}
