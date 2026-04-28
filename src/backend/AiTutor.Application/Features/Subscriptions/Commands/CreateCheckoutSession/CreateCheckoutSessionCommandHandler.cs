using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Domain.Entities;
using AiTutor.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Subscriptions.Commands.CreateCheckoutSession;

public class CreateCheckoutSessionCommandHandler
    : IRequestHandler<CreateCheckoutSessionCommand, Result<CheckoutSessionResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly IStripeService _stripe;

    public CreateCheckoutSessionCommandHandler(
        IApplicationDbContext context,
        IStripeService stripe)
    {
        _context = context;
        _stripe = stripe;
    }

    public async Task<Result<CheckoutSessionResponse>> Handle(
        CreateCheckoutSessionCommand request, CancellationToken ct)
    {
        // 1. User trebuie să existe
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, ct);

        if (user is null)
            throw new NotFoundException(nameof(User), request.UserId);

        // 2. Blocăm dacă user-ul are deja un subscription Active sau Pending pe același plan
        var hasActiveOrPending = await _context.Subscriptions
            .AnyAsync(s =>
                s.UserId == request.UserId &&
                s.Type == request.SubscriptionType &&
                (s.Status == SubscriptionStatus.Active ||
                 s.Status == SubscriptionStatus.Pending), ct);

        if (hasActiveOrPending)
        {
            return Result<CheckoutSessionResponse>.Failure(
                "User already has an active or pending subscription for this plan.",
                409);
        }

        // 3. Creăm sesiunea Stripe Checkout
        var checkoutResult = await _stripe.CreateCheckoutSessionAsync(
            userId: request.UserId,
            userEmail: user.Email.Value,
            subscriptionType: request.SubscriptionType,
            ct: ct);

        // 4. Persistăm un Subscription Pending — îl vom finaliza la webhook
        var pendingSubscription = Subscription.CreatePending(
            userId: request.UserId,
            type: request.SubscriptionType,
            price: ResolvePriceFor(request.SubscriptionType),
            stripePriceId: checkoutResult.StripePriceId,
            stripeCustomerId: string.IsNullOrWhiteSpace(checkoutResult.StripeCustomerId)
                ? null
                : checkoutResult.StripeCustomerId);

        _context.Subscriptions.Add(pendingSubscription);
        await _context.SaveChangesAsync(ct);

        return Result<CheckoutSessionResponse>.Success(
            new CheckoutSessionResponse(
                SessionId: checkoutResult.SessionId,
                CheckoutUrl: checkoutResult.CheckoutUrl),
            201);
    }

    /// <summary>
    /// Prețuri afișate (RON) — sursa de adevăr rămâne Stripe (price-ul real
    /// e setat în dashboard). Aici stocăm o valoare de referință în DB pentru
    /// rapoarte și UI offline. Dacă ai prețuri diferite, modifică aici.
    /// </summary>
    private static decimal ResolvePriceFor(SubscriptionType type) => type switch
    {
        SubscriptionType.ParentMonthly => 49.99m,
        SubscriptionType.ParentYearly => 499.99m,
        SubscriptionType.School => 999.99m,
        _ => 0m
    };
}
