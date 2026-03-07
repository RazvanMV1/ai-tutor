using AiTutor.Application.Common.Models;
using MediatR;

namespace AiTutor.Application.Features.Subscriptions.Commands.CancelSubscription;

public record CancelSubscriptionCommand(Guid SubscriptionId, Guid UserId)
    : IRequest<Result<bool>>;
