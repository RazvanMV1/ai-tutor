namespace AiTutor.Infrastructure.Services.Stripe;

/// <summary>
/// Strongly-typed bind pentru secțiunea "Stripe" din appsettings.json.
/// Folosit prin IOptions&lt;StripeOptions&gt; în StripeService.
/// </summary>
public class StripeOptions
{
    public const string SectionName = "Stripe";

    public string SecretKey { get; set; } = string.Empty;
    public string PublishableKey { get; set; } = string.Empty;
    public string WebhookSecret { get; set; } = string.Empty;
    public string SuccessUrl { get; set; } = string.Empty;
    public string CancelUrl { get; set; } = string.Empty;
    public StripePriceMap Prices { get; set; } = new();
}

public class StripePriceMap
{
    public string ParentMonthly { get; set; } = string.Empty;
    public string ParentYearly { get; set; } = string.Empty;
    public string School { get; set; } = string.Empty;
}
