using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Application.Common.Models.Stripe;
using AiTutor.Domain.Entities;
using AiTutor.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AiTutor.Application.Features.Subscriptions.Commands.HandleStripeWebhook;

public class HandleStripeWebhookCommandHandler
    : IRequestHandler<HandleStripeWebhookCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;
    private readonly IStripeService _stripe;
    private readonly ILogger<HandleStripeWebhookCommandHandler> _logger;

    public HandleStripeWebhookCommandHandler(
        IApplicationDbContext context,
        IStripeService stripe,
        ILogger<HandleStripeWebhookCommandHandler> logger)
    {
        _context = context;
        _stripe = stripe;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(
        HandleStripeWebhookCommand request, CancellationToken ct)
    {
        // 1. Validează semnătura și parsează evenimentul.
        // ConstructWebhookEvent aruncă StripeException dacă semnătura e invalidă —
        // controller-ul prinde și returnează 400. Asta e protecția anti-spoofing.
        StripeWebhookEvent evt;
        try
        {
            evt = _stripe.ConstructWebhookEvent(request.Payload, request.SignatureHeader);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Stripe webhook signature validation failed.");
            return Result<bool>.Failure("Invalid webhook signature.", 400);
        }

        _logger.LogInformation(
            "Received Stripe webhook {EventId} of type {EventType}",
            evt.Id, evt.Type);

        // 2. Dispatch pe tipul de eveniment
        switch (evt.Type)
        {
            case "checkout.session.completed":
                await HandleCheckoutCompleted(evt, ct);
                break;

            case "customer.subscription.created":
            case "customer.subscription.updated":
                await HandleSubscriptionUpdated(evt, ct);
                break;

            case "customer.subscription.deleted":
                await HandleSubscriptionDeleted(evt, ct);
                break;

            default:
                // Stripe trimite multe alte eventuri (invoice.*, payment_intent.*, etc.)
                // — acceptăm cu 200 OK ca să nu retrimită, dar nu facem nimic.
                _logger.LogDebug("Ignoring Stripe event type {Type}", evt.Type);
                break;
        }

        return Result<bool>.Success(true);
    }

    // ===== Handlers per event type =====

    private async Task HandleCheckoutCompleted(StripeWebhookEvent evt, CancellationToken ct)
    {
        if (evt.CheckoutSession is null)
        {
            _logger.LogWarning("checkout.session.completed without session data — skipping.");
            return;
        }

        var data = evt.CheckoutSession;

        if (data.AppUserId is null)
        {
            _logger.LogWarning(
                "checkout.session.completed without app_user_id metadata (session {Sid}) — skipping.",
                data.SessionId);
            return;
        }

        // Găsim Subscription-ul Pending pe care l-am creat la CreateCheckoutSession.
        // Strategie de matching: cel mai recent Pending al user-ului pe acest plan.
        // (Nu avem încă SessionId stocat; webhook-ul e prima ocazie să-l legăm.)
        var pending = await _context.Subscriptions
            .Where(s =>
                s.UserId == data.AppUserId.Value &&
                s.Status == SubscriptionStatus.Pending)
            .OrderByDescending(s => s.CreatedAt)
            .FirstOrDefaultAsync(ct);

        if (pending is null)
        {
            _logger.LogWarning(
                "No pending subscription found for user {UserId} (session {Sid}).",
                data.AppUserId, data.SessionId);
            return;
        }

        // Idempotency: dacă am procesat deja acest event, nu repeta.
        if (pending.HasProcessedEvent(evt.Id))
        {
            _logger.LogInformation("Webhook event {EventId} already processed.", evt.Id);
            return;
        }

        // Legăm subscription-ul nostru de cel real Stripe.
        if (!string.IsNullOrWhiteSpace(data.SubscriptionId) &&
            !string.IsNullOrWhiteSpace(data.CustomerId))
        {
            pending.LinkStripe(data.CustomerId, data.SubscriptionId);
        }

        pending.RecordStripeEvent(evt.Id);
        await _context.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Linked subscription {SubId} to Stripe (customer {Cid}, sub {Ssid}).",
            pending.Id, data.CustomerId, data.SubscriptionId);

        // Nu marcăm Active aici — așteptăm customer.subscription.updated cu status="active",
        // care vine imediat după și conține perioada exactă (CurrentPeriodStart/End).
    }

    private async Task HandleSubscriptionUpdated(StripeWebhookEvent evt, CancellationToken ct)
    {
        if (evt.Subscription is null)
        {
            _logger.LogWarning("subscription.updated without subscription data — skipping.");
            return;
        }

        var data = evt.Subscription;

        var subscription = await FindSubscription(data, ct);
        if (subscription is null)
        {
            _logger.LogWarning(
                "No local subscription matched Stripe sub {Ssid}.",
                data.SubscriptionId);
            return;
        }

        if (subscription.HasProcessedEvent(evt.Id))
        {
            _logger.LogInformation("Webhook event {EventId} already processed.", evt.Id);
            return;
        }

        // Asigurăm-ne că link-ul e setat (în caz că webhook-ul subscription.updated
        // ajunge înaintea checkout.session.completed — Stripe nu garantează ordinea).
        if (string.IsNullOrWhiteSpace(subscription.StripeSubscriptionId))
        {
            subscription.LinkStripe(data.CustomerId, data.SubscriptionId);
        }

        // Mapează statusul Stripe → SubscriptionStatus și actualizează entitatea.
        var domainStatus = StripeStatusMapper.ToDomainStatus(data.Status);

        switch (domainStatus)
        {
            case SubscriptionStatus.Active:
                subscription.MarkActive(data.CurrentPeriodStart, data.CurrentPeriodEnd);
                break;

            case SubscriptionStatus.PastDue:
                subscription.MarkPastDue();
                break;

            case SubscriptionStatus.Incomplete:
                subscription.MarkIncomplete();
                break;

            case SubscriptionStatus.Canceled:
                subscription.MarkCanceledByStripe();
                break;

            default:
                _logger.LogDebug(
                    "Unmapped Stripe status '{Status}' → ignoring update.",
                    data.Status);
                break;
        }

        subscription.RecordStripeEvent(evt.Id);
        await _context.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Subscription {SubId} updated to {Status} from Stripe event {EventId}.",
            subscription.Id, domainStatus, evt.Id);
    }

    private async Task HandleSubscriptionDeleted(StripeWebhookEvent evt, CancellationToken ct)
    {
        if (evt.Subscription is null) return;

        var subscription = await FindSubscription(evt.Subscription, ct);
        if (subscription is null) return;

        if (subscription.HasProcessedEvent(evt.Id)) return;

        subscription.MarkCanceledByStripe();
        subscription.RecordStripeEvent(evt.Id);
        await _context.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Subscription {SubId} canceled via Stripe event {EventId}.",
            subscription.Id, evt.Id);
    }

    // ===== Helpers =====

    /// <summary>
    /// Găsește subscription-ul local matched cu cel Stripe.
    /// Strategia: întâi după StripeSubscriptionId (cazul normal),
    /// apoi fallback la cel mai recent Pending al user-ului
    /// (când webhook-ul ajunge înaintea linking-ului).
    /// </summary>
    private async Task<Subscription?> FindSubscription(
        StripeSubscriptionData data, CancellationToken ct)
    {
        // 1. Match direct după StripeSubscriptionId
        var matched = await _context.Subscriptions
            .FirstOrDefaultAsync(s => s.StripeSubscriptionId == data.SubscriptionId, ct);

        if (matched is not null) return matched;

        // 2. Fallback: cel mai recent Pending al user-ului (dacă avem app_user_id în metadata)
        if (data.AppUserId is null) return null;

        return await _context.Subscriptions
            .Where(s =>
                s.UserId == data.AppUserId.Value &&
                s.Status == SubscriptionStatus.Pending)
            .OrderByDescending(s => s.CreatedAt)
            .FirstOrDefaultAsync(ct);
    }
}
