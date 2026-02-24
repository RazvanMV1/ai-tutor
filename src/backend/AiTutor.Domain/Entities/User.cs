using AiTutor.Domain.Common;
using AiTutor.Domain.Enums;
using AiTutor.Domain.Events;
using AiTutor.Domain.ValueObjects;

namespace AiTutor.Domain.Entities;

public class User : BaseEntity
{
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public Email Email { get; private set; } = null!;
    public UserRole Role { get; private set; }
    public bool IsActive { get; private set; } = true;
    public string? IdentityUserId { get; private set; }

    private readonly List<StudentProgress> _progresses = new();
    public IReadOnlyCollection<StudentProgress> Progresses => _progresses.AsReadOnly();

    private User() { }

    public static User Create(string firstName, string lastName, string email, UserRole role)
    {
        var user = new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = Email.Create(email),
            Role = role
        };

        user.AddDomainEvent(new UserCreatedEvent(user.Id, email));
        return user;
    }

    public void UpdateProfile(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
        SetUpdatedAt();
    }

    public void Deactivate()
    {
        IsActive = false;
        SetUpdatedAt();
    }

    public void SetIdentityUserId(string identityUserId)
    {
        IdentityUserId = identityUserId;
        SetUpdatedAt();
    }

    public string FullName => $"{FirstName} {LastName}";
}
