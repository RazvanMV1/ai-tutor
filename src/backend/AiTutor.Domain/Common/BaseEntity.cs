using MediatR;

namespace AiTutor.Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; protected set; }

    private readonly List<INotification> _domainEvents = new();
    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(INotification domainEvent) =>
        _domainEvents.Add(domainEvent);

    public void RemoveDomainEvent(INotification domainEvent) =>
        _domainEvents.Remove(domainEvent);

    public void ClearDomainEvents() =>
        _domainEvents.Clear();

    public void SetUpdatedAt() =>
        UpdatedAt = DateTime.UtcNow;
}
