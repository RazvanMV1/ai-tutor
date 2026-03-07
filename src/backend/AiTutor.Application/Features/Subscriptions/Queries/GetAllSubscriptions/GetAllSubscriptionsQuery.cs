using AiTutor.Application.Common.Models;
using AiTutor.Application.Features.Subscriptions.Commands.CreateSubscription;
using MediatR;

namespace AiTutor.Application.Features.Subscriptions.Queries.GetAllSubscriptions;

public record GetAllSubscriptionsQuery(int Page = 1, int PageSize = 20)
    : IRequest<Result<PaginatedList<SubscriptionResponse>>>;
