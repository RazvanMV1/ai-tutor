using AiTutor.Domain.Common;
using AiTutor.Domain.Enums;
using AiTutor.Domain.Exceptions;

namespace AiTutor.Domain.Entities;

public class Subscription : BaseEntity
{
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;
    public SubscriptionType Type { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public bool IsActive { get; private set; }
    public decimal Price { get; private set; }

    // === Stripe integration fields ===
    public SubscriptionStatus Status { get; private set; }
    public string? StripeCustomerId { get; private set; }
    public string? StripeSubscriptionId { get; private set; }
    public string? StripePriceId { get; private set; }
    public string? LastStripeEventId { get; private set; }

    private Subscription() { }

    /// <summary>
    /// Constructor existent — folosit de seed și de fluxurile legacy (fără Stripe).
    /// Creează un subscription deja Active.
    /// </summary>
    public static Subscription Create(Guid userId, SubscriptionType type,
        DateTime startDate, DateTime endDate, decimal price)
    {
        if (endDate <= startDate)
            throw new DomainException("End date must be after start date.");

        if (price < 0)
            throw new DomainException("Price cannot be negative.");

        return new Subscription
        {
            UserId = userId,
            Type = type,
            StartDate = startDate,
            EndDate = endDate,
            IsActive = true,
            Price = price,
            Status = SubscriptionStatus.Active
        };
    }

    /// <summary>
    /// Constructor nou — folosit la inițierea unui Stripe Checkout Session.
    /// Creează un subscription în Pending, până la confirmarea webhook-ului.
    /// </summary>
    public static Subscription CreatePending(
        Guid userId,
        SubscriptionType type,
        decimal price,
        string stripePriceId,
        string? stripeCustomerId = null)
    {
        if (price < 0)
            throw new DomainException("Price cannot be negative.");

        if (string.IsNullOrWhiteSpace(stripePriceId))
            throw new DomainException("Stripe price id is required.");

        var now = DateTime.UtcNow;

        return new Subscription
        {
            UserId = userId,
            Type = type,
            StartDate = now,
            EndDate = now, // va fi setat la confirmarea webhook-ului
            IsActive = false,
            Price = price,
            Status = SubscriptionStatus.Pending,
            StripePriceId = stripePriceId,
            StripeCustomerId = stripeCustomerId
        };
    }

    public bool IsValid() => IsActive && DateTime.UtcNow <= EndDate;

    public void Deactivate()
    {
        IsActive = false;
        Status = SubscriptionStatus.Canceled;
        SetUpdatedAt();
    }

    public void Extend(DateTime newEndDate)
    {
        if (newEndDate <= EndDate)
            throw new DomainException("New end date must be after current end date.");

        EndDate = newEndDate;
        SetUpdatedAt();
    }

    // === Stripe lifecycle methods ===

    /// <summary>
    /// Apelat la primirea checkout.session.completed.
    /// Leagă subscription-ul nostru de cel real din Stripe.
    /// </summary>
    public void LinkStripe(string stripeCustomerId, string stripeSubscriptionId)
    {
        if (string.IsNullOrWhiteSpace(stripeCustomerId))
            throw new DomainException("Stripe customer id is required.");
        if (string.IsNullOrWhiteSpace(stripeSubscriptionId))
            throw new DomainException("Stripe subscription id is required.");

        StripeCustomerId = stripeCustomerId;
        StripeSubscriptionId = stripeSubscriptionId;
        SetUpdatedAt();
    }

    /// <summary>
    /// Apelat la customer.subscription.updated când Stripe ne anunță "active".
    /// </summary>
    public void MarkActive(DateTime currentPeriodStart, DateTime currentPeriodEnd)
    {
        if (currentPeriodEnd <= currentPeriodStart)
            throw new DomainException("Period end must be after period start.");

        StartDate = currentPeriodStart;
        EndDate = currentPeriodEnd;
        IsActive = true;
        Status = SubscriptionStatus.Active;
        SetUpdatedAt();
    }

    public void MarkPastDue()
    {
        Status = SubscriptionStatus.PastDue;
        // IsActive rămâne true — utilizatorul are încă acces în perioada de grace
        SetUpdatedAt();
    }

    public void MarkIncomplete()
    {
        Status = SubscriptionStatus.Incomplete;
        IsActive = false;
        SetUpdatedAt();
    }

    public void MarkCanceledByStripe()
    {
        IsActive = false;
        Status = SubscriptionStatus.Canceled;
        SetUpdatedAt();
    }

    /// <summary>
    /// Idempotency helper — evită procesarea aceluiași webhook event de două ori.
    /// </summary>
    public bool HasProcessedEvent(string stripeEventId)
        => !string.IsNullOrEmpty(LastStripeEventId)
           && LastStripeEventId == stripeEventId;

    public void RecordStripeEvent(string stripeEventId)
    {
        LastStripeEventId = stripeEventId;
        SetUpdatedAt();
    }
}
