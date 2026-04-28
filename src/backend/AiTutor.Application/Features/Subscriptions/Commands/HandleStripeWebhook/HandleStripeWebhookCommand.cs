using AiTutor.Application.Common.Models;
using MediatR;

namespace AiTutor.Application.Features.Subscriptions.Commands.HandleStripeWebhook;

/// <summary>
/// Procesează un webhook event Stripe primit raw.
/// Payload-ul JSON și header-ul de semnătură vin de la controller, neatinse.
/// </summary>
public record HandleStripeWebhookCommand(
    string Payload,
    string SignatureHeader
) : IRequest<Result<bool>>;
