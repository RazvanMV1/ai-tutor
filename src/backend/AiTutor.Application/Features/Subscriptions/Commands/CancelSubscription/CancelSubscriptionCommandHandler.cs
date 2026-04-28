using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AiTutor.Application.Features.Subscriptions.Commands.CancelSubscription;

public class CancelSubscriptionCommandHandler : IRequestHandler<CancelSubscriptionCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;
    private readonly IStripeService _stripe;
    private readonly ILogger<CancelSubscriptionCommandHandler> _logger;

    public CancelSubscriptionCommandHandler(
        IApplicationDbContext context,
        IStripeService stripe,
        ILogger<CancelSubscriptionCommandHandler> logger)
    {
        _context = context;
        _stripe = stripe;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(CancelSubscriptionCommand request, CancellationToken ct)
    {
        var subscription = await _context.Subscriptions
            .FirstOrDefaultAsync(s => s.Id == request.SubscriptionId, ct);

        if (subscription is null)
            throw new NotFoundException(nameof(Subscription), request.SubscriptionId);

        if (subscription.UserId != request.UserId)
            throw new ForbiddenAccessException();

        // Dacă subscription-ul este legat de Stripe, anulăm întâi acolo.
        // Stripe va trimite ulterior customer.subscription.deleted (sau .updated cu
        // cancel_at_period_end=true) — webhook-ul nostru gestionează idempotent.
        if (!string.IsNullOrWhiteSpace(subscription.StripeSubscriptionId))
        {
            try
            {
                await _stripe.CancelSubscriptionAsync(
                    stripeSubscriptionId: subscription.StripeSubscriptionId,
                    cancelImmediately: false, // anulare la sfârșitul perioadei plătite
                    ct: ct);

                _logger.LogInformation(
                    "Stripe subscription {Ssid} scheduled for cancellation.",
                    subscription.StripeSubscriptionId);
            }
            catch (Exception ex)
            {
                // Nu blocăm anularea locală dacă Stripe e indisponibil — log + continuă.
                // Operatorul poate sincroniza manual ulterior. Alternativ, am putea
                // returna Failure 502 ca să forțăm retry-ul de la frontend.
                _logger.LogError(ex,
                    "Failed to cancel Stripe subscription {Ssid}. Continuing with local cancellation.",
                    subscription.StripeSubscriptionId);
            }
        }

        subscription.Deactivate();
        await _context.SaveChangesAsync(ct);

        return Result<bool>.Success(true);
    }
}
