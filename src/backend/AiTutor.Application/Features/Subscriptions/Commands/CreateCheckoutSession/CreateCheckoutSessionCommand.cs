using AiTutor.Application.Common.Models;
using AiTutor.Domain.Enums;
using MediatR;

namespace AiTutor.Application.Features.Subscriptions.Commands.CreateCheckoutSession;

public record CreateCheckoutSessionCommand(
    Guid UserId,
    SubscriptionType SubscriptionType
) : IRequest<Result<CheckoutSessionResponse>>;

public record CheckoutSessionResponse(
    string SessionId,
    string CheckoutUrl);
