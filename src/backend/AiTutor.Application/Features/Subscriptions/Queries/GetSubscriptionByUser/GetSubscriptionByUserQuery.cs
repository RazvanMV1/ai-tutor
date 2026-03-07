using AiTutor.Application.Common.Models;
using AiTutor.Application.Features.Subscriptions.Commands.CreateSubscription;
using MediatR;

namespace AiTutor.Application.Features.Subscriptions.Queries.GetSubscriptionByUser;

public record GetSubscriptionByUserQuery(Guid UserId)
    : IRequest<Result<List<SubscriptionResponse>>>;
