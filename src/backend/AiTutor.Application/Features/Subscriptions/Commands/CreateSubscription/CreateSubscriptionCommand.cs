using AiTutor.Application.Common.Models;
using AiTutor.Domain.Enums;
using MediatR;

namespace AiTutor.Application.Features.Subscriptions.Commands.CreateSubscription;

public record CreateSubscriptionCommand(
    Guid UserId,
    SubscriptionType Type,
    decimal Price,
    DateTime StartDate,
    DateTime EndDate
) : IRequest<Result<SubscriptionResponse>>;

public record SubscriptionResponse(
    Guid Id,
    Guid UserId,
    SubscriptionType Type,
    decimal Price,
    DateTime StartDate,
    DateTime EndDate,
    bool IsActive
);
