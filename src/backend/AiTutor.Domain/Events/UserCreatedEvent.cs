using MediatR;

namespace AiTutor.Domain.Events;

public record UserCreatedEvent(Guid UserId, string Email) : INotification;
