using AiTutor.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AiTutor.Application.Features.Users.Events;

public class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
{
    private readonly ILogger<UserCreatedEventHandler> _logger;

    public UserCreatedEventHandler(ILogger<UserCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "User {UserId} created with email {Email}",
            notification.UserId,
            notification.Email);

        return Task.CompletedTask;
    }
}
