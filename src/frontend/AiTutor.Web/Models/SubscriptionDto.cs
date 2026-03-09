namespace AiTutor.Web.Models;

// ⚠️ Are price în plus
public record SubscriptionDto(
    Guid Id,
    Guid UserId,
    int Type,
    double Price,
    DateTime StartDate,
    DateTime EndDate,
    bool IsActive)
{
    public string TypeName => Type switch
    {
        1 => "Free",
        2 => "Părinte Lunar",
        3 => "Părinte Anual",
        4 => "Școală",
        _ => "Necunoscut"
    };
}

public record CreateSubscriptionRequest(
    Guid UserId,
    int Type,
    DateTime StartDate,
    DateTime EndDate);
