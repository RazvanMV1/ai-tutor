using AiTutor.Domain.Enums;

namespace AiTutor.Application.Common.Models.Stripe;

/// <summary>
/// Rezultatul creării unei Stripe Checkout Session.
/// </summary>
public record CheckoutSessionResult(
    string SessionId,
    string CheckoutUrl,
    string StripeCustomerId,
    string StripePriceId);

/// <summary>
/// Reprezentare neutră a unui webhook event Stripe, decuplată de SDK.
/// Tipurile pe care le procesăm:
///  - "checkout.session.completed"
///  - "customer.subscription.updated"
///  - "customer.subscription.deleted"
///  - "invoice.payment_failed"
/// </summary>
public record StripeWebhookEvent(
    string Id,
    string Type,
    StripeSubscriptionData? Subscription,
    StripeCheckoutSessionData? CheckoutSession);

/// <summary>
/// Datele extrase dintr-un eveniment customer.subscription.* sau invoice.*
/// </summary>
public record StripeSubscriptionData(
    string SubscriptionId,
    string CustomerId,
    string PriceId,
    string Status, // raw Stripe status: "active", "past_due", "canceled", "incomplete", "trialing", etc.
    DateTime CurrentPeriodStart,
    DateTime CurrentPeriodEnd,
    Guid? AppUserId, // extras din metadata
    SubscriptionType? AppSubscriptionType); // extras din metadata

/// <summary>
/// Datele extrase dintr-un eveniment checkout.session.completed
/// </summary>
public record StripeCheckoutSessionData(
    string SessionId,
    string CustomerId,
    string SubscriptionId,
    Guid? AppUserId,
    SubscriptionType? AppSubscriptionType);

/// <summary>
/// Helper pentru maparea statusului Stripe la enum-ul nostru de domeniu.
/// </summary>
public static class StripeStatusMapper
{
    public static SubscriptionStatus ToDomainStatus(string stripeStatus) =>
        stripeStatus?.ToLowerInvariant() switch
        {
            "active" or "trialing" => SubscriptionStatus.Active,
            "past_due" => SubscriptionStatus.PastDue,
            "canceled" or "unpaid" => SubscriptionStatus.Canceled,
            "incomplete" or "incomplete_expired" => SubscriptionStatus.Incomplete,
            _ => SubscriptionStatus.Pending
        };
}
