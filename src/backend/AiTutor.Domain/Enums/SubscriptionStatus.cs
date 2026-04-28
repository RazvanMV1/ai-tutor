namespace AiTutor.Domain.Enums;

/// <summary>
/// Reflectă starea unui Subscription, sincronizată cu Stripe.
/// Mapare către Stripe subscription statuses:
/// - Pending     → înainte de checkout.session.completed
/// - Active      → "active" sau "trialing" în Stripe
/// - PastDue     → "past_due" în Stripe (plată eșuată, încă activ)
/// - Canceled    → "canceled" în Stripe (sau anulat manual)
/// - Incomplete  → "incomplete" / "incomplete_expired" în Stripe
/// </summary>
public enum SubscriptionStatus
{
    Pending = 1,
    Active = 2,
    PastDue = 3,
    Canceled = 4,
    Incomplete = 5
}
