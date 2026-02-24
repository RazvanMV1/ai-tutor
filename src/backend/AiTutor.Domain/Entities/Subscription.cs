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

    private Subscription() { }

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
            Price = price
        };
    }

    public bool IsValid() => IsActive && DateTime.UtcNow <= EndDate;

    public void Deactivate()
    {
        IsActive = false;
        SetUpdatedAt();
    }

    public void Extend(DateTime newEndDate)
    {
        if (newEndDate <= EndDate)
            throw new DomainException("New end date must be after current end date.");

        EndDate = newEndDate;
        SetUpdatedAt();
    }
}
