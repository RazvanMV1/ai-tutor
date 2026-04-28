using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models.Stripe;
using AiTutor.Domain.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace AiTutor.Infrastructure.Services.Stripe;

public class StripeService : IStripeService
{
    private const string MetadataUserIdKey = "app_user_id";
    private const string MetadataSubscriptionTypeKey = "app_subscription_type";

    private readonly StripeOptions _options;
    private readonly ILogger<StripeService> _logger;

    public StripeService(IOptions<StripeOptions> options, ILogger<StripeService> logger)
    {
        _options = options.Value;
        _logger = logger;

        // Stripe.net folosește o cheie statică globală.
        // O setăm o singură dată (DI service e Singleton).
        if (!string.IsNullOrWhiteSpace(_options.SecretKey))
        {
            StripeConfiguration.ApiKey = _options.SecretKey;
        }
    }

    public async Task<CheckoutSessionResult> CreateCheckoutSessionAsync(
        Guid userId,
        string userEmail,
        SubscriptionType subscriptionType,
        CancellationToken ct = default)
    {
        var priceId = GetPriceIdFor(subscriptionType);

        var sessionOptions = new SessionCreateOptions
        {
            Mode = "subscription",
            SuccessUrl = _options.SuccessUrl,
            CancelUrl = _options.CancelUrl,
            CustomerEmail = userEmail,
            ClientReferenceId = userId.ToString(),
            LineItems = new List<SessionLineItemOptions>
            {
                new()
                {
                    Price = priceId,
                    Quantity = 1
                }
            },
            // Metadata pe Session (vizibilă în checkout.session.completed)
            Metadata = new Dictionary<string, string>
            {
                [MetadataUserIdKey] = userId.ToString(),
                [MetadataSubscriptionTypeKey] = ((int)subscriptionType).ToString()
            },
            // Metadata pe Subscription-ul creat (vizibilă în customer.subscription.* events)
            SubscriptionData = new SessionSubscriptionDataOptions
            {
                Metadata = new Dictionary<string, string>
                {
                    [MetadataUserIdKey] = userId.ToString(),
                    [MetadataSubscriptionTypeKey] = ((int)subscriptionType).ToString()
                }
            }
        };

        var service = new SessionService();
        var session = await service.CreateAsync(sessionOptions, cancellationToken: ct);

        _logger.LogInformation(
            "Created Stripe Checkout Session {SessionId} for user {UserId}, plan {Plan}",
            session.Id, userId, subscriptionType);

        return new CheckoutSessionResult(
            SessionId: session.Id,
            CheckoutUrl: session.Url,
            StripeCustomerId: session.CustomerId ?? string.Empty,
            StripePriceId: priceId);
    }

    public async Task CancelSubscriptionAsync(
        string stripeSubscriptionId,
        bool cancelImmediately = false,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(stripeSubscriptionId))
            throw new ArgumentException("Stripe subscription id is required.", nameof(stripeSubscriptionId));

        var service = new SubscriptionService();

        if (cancelImmediately)
        {
            await service.CancelAsync(
                stripeSubscriptionId,
                new SubscriptionCancelOptions(),
                cancellationToken: ct);
        }
        else
        {
            // Anulare la sfârșitul perioadei curente (utilizatorul păstrează accesul plătit).
            await service.UpdateAsync(
                stripeSubscriptionId,
                new SubscriptionUpdateOptions { CancelAtPeriodEnd = true },
                cancellationToken: ct);
        }

        _logger.LogInformation(
            "Canceled Stripe subscription {SubscriptionId} (immediate={Immediate})",
            stripeSubscriptionId, cancelImmediately);
    }

    public StripeWebhookEvent ConstructWebhookEvent(string payload, string signatureHeader)
    {
        if (string.IsNullOrWhiteSpace(_options.WebhookSecret))
            throw new InvalidOperationException("Stripe webhook secret is not configured.");

        // Aruncă StripeException dacă semnătura nu se potrivește — anti-spoofing.
        var stripeEvent = EventUtility.ConstructEvent(
            json: payload,
            stripeSignatureHeader: signatureHeader,
            secret: _options.WebhookSecret);

        return MapEvent(stripeEvent);
    }

    public string GetPriceIdFor(SubscriptionType subscriptionType) =>
        subscriptionType switch
        {
            SubscriptionType.ParentMonthly => RequirePrice(_options.Prices.ParentMonthly, subscriptionType),
            SubscriptionType.ParentYearly => RequirePrice(_options.Prices.ParentYearly, subscriptionType),
            SubscriptionType.School => RequirePrice(_options.Prices.School, subscriptionType),
            SubscriptionType.Free => throw new InvalidOperationException(
                "Free subscription does not require a Stripe checkout."),
            _ => throw new InvalidOperationException(
                $"Unknown SubscriptionType: {subscriptionType}")
        };

    // ===== private helpers =====

    private static string RequirePrice(string priceId, SubscriptionType type)
    {
        if (string.IsNullOrWhiteSpace(priceId) || priceId.Contains("REPLACE_ME"))
            throw new InvalidOperationException(
                $"Stripe price id for {type} is not configured.");
        return priceId;
    }

    private StripeWebhookEvent MapEvent(Event stripeEvent)
    {
        StripeSubscriptionData? subscriptionData = null;
        StripeCheckoutSessionData? checkoutData = null;

        switch (stripeEvent.Type)
        {
            case "checkout.session.completed":
                if (stripeEvent.Data.Object is Session session)
                {
                    checkoutData = new StripeCheckoutSessionData(
                        SessionId: session.Id,
                        CustomerId: session.CustomerId ?? string.Empty,
                        SubscriptionId: session.SubscriptionId ?? string.Empty,
                        AppUserId: TryParseUserId(session.Metadata),
                        AppSubscriptionType: TryParseSubscriptionType(session.Metadata));
                }
                break;

            case "customer.subscription.created":
            case "customer.subscription.updated":
            case "customer.subscription.deleted":
                if (stripeEvent.Data.Object is Subscription sub)
                {
                    subscriptionData = MapSubscription(sub);
                }
                break;

            case "invoice.payment_failed":
                // Stripe trimite și un customer.subscription.updated cu status=past_due,
                // dar logăm și aici pentru observabilitate.
                _logger.LogWarning("Stripe invoice.payment_failed event {EventId}", stripeEvent.Id);
                break;

            default:
                _logger.LogDebug("Unhandled Stripe event type: {Type}", stripeEvent.Type);
                break;
        }

        return new StripeWebhookEvent(
            Id: stripeEvent.Id,
            Type: stripeEvent.Type,
            Subscription: subscriptionData,
            CheckoutSession: checkoutData);
    }

    private static StripeSubscriptionData MapSubscription(Subscription sub)
    {
        // Stripe API 2025-03-31 (Basil) a mutat current_period_start/end de pe Subscription
        // pe SubscriptionItem (fiecare item are propria perioadă de billing).
        // Noi avem un singur item per subscription (un plan), deci luăm primul.
        var firstItem = sub.Items?.Data?.FirstOrDefault();
        var priceId = firstItem?.Price?.Id ?? string.Empty;

        var periodStart = firstItem?.CurrentPeriodStart ?? DateTime.UtcNow;
        var periodEnd = firstItem?.CurrentPeriodEnd ?? DateTime.UtcNow;

        return new StripeSubscriptionData(
            SubscriptionId: sub.Id,
            CustomerId: sub.CustomerId,
            PriceId: priceId,
            Status: sub.Status,
            CurrentPeriodStart: periodStart,
            CurrentPeriodEnd: periodEnd,
            AppUserId: TryParseUserId(sub.Metadata),
            AppSubscriptionType: TryParseSubscriptionType(sub.Metadata));
    }

    private static Guid? TryParseUserId(IDictionary<string, string>? metadata)
    {
        if (metadata is null) return null;
        if (!metadata.TryGetValue(MetadataUserIdKey, out var raw)) return null;
        return Guid.TryParse(raw, out var id) ? id : null;
    }

    private static SubscriptionType? TryParseSubscriptionType(IDictionary<string, string>? metadata)
    {
        if (metadata is null) return null;
        if (!metadata.TryGetValue(MetadataSubscriptionTypeKey, out var raw)) return null;
        return int.TryParse(raw, out var num) && Enum.IsDefined(typeof(SubscriptionType), num)
            ? (SubscriptionType)num
            : null;
    }
}
