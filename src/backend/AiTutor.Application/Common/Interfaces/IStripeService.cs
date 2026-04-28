using AiTutor.Application.Common.Models.Stripe;
using AiTutor.Domain.Enums;

namespace AiTutor.Application.Common.Interfaces;

/// <summary>
/// Abstracție peste Stripe API. Implementarea concretă (cu Stripe.net) trăiește
/// în Infrastructure. Application Layer nu trebuie să cunoască SDK-ul Stripe.
/// </summary>
public interface IStripeService
{
    /// <summary>
    /// Creează o sesiune Stripe Checkout pentru un plan dat și returnează URL-ul
    /// la care frontend-ul redirectează utilizatorul.
    /// </summary>
    Task<CheckoutSessionResult> CreateCheckoutSessionAsync(
        Guid userId,
        string userEmail,
        SubscriptionType subscriptionType,
        CancellationToken ct = default);

    /// <summary>
    /// Anulează un Stripe Subscription. Anularea se aplică imediat sau la sfârșitul
    /// perioadei curente, în funcție de parametru.
    /// </summary>
    Task CancelSubscriptionAsync(
        string stripeSubscriptionId,
        bool cancelImmediately = false,
        CancellationToken ct = default);

    /// <summary>
    /// Validează semnătura HTTP a unui payload webhook și deserializează evenimentul.
    /// Aruncă excepție dacă semnătura este invalidă (anti-spoofing).
    /// </summary>
    StripeWebhookEvent ConstructWebhookEvent(string payload, string signatureHeader);

    /// <summary>
    /// Mapează un SubscriptionType la price ID-ul Stripe configurat în appsettings.
    /// Aruncă dacă tipul nu are un price configurat (ex. Free).
    /// </summary>
    string GetPriceIdFor(SubscriptionType subscriptionType);
}
