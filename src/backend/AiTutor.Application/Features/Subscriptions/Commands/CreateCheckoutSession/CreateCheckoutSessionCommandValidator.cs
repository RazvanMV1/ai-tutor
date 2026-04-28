using AiTutor.Domain.Enums;
using FluentValidation;

namespace AiTutor.Application.Features.Subscriptions.Commands.CreateCheckoutSession;

public class CreateCheckoutSessionCommandValidator
    : AbstractValidator<CreateCheckoutSessionCommand>
{
    public CreateCheckoutSessionCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User id is required.");

        RuleFor(x => x.SubscriptionType)
            .IsInEnum()
            .WithMessage("Invalid subscription type.")
            .NotEqual(SubscriptionType.Free)
            .WithMessage("Free plan does not require a checkout session.");
    }
}
