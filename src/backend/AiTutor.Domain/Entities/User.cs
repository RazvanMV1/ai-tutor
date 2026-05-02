using AiTutor.Domain.Common;
using AiTutor.Domain.Enums;
using AiTutor.Domain.Events;
using AiTutor.Domain.ValueObjects;
using System.Security.Cryptography;

namespace AiTutor.Domain.Entities;

public class User : BaseEntity
{
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public Email Email { get; private set; } = null!;
    public UserRole Role { get; private set; }
    public bool IsActive { get; private set; } = true;
    public string? IdentityUserId { get; private set; }

    // ðŸ†• Parent â†” Child relationship
    public Guid? ParentId { get; private set; }       // Set on Student when linked to a Parent
    public string? InvitationCode { get; private set; } // Set on Parent (e.g. "PAR-X7K9M2")

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

        // Auto-generate invitation code only for Parents
        if (role == UserRole.Parent)
            user.InvitationCode = GenerateInvitationCode();

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

    // ðŸ†• Link this Student to a Parent (called on a Student entity)
    public void LinkToParent(Guid parentId)
    {
        if (Role != UserRole.Student)
            throw new InvalidOperationException("Only students can be linked to a parent.");
        ParentId = parentId;
        SetUpdatedAt();
    }

    public void UnlinkFromParent()
    {
        ParentId = null;
        SetUpdatedAt();
    }

    // ðŸ†• Re-generate invitation code if needed
    public void RegenerateInvitationCode()
    {
        if (Role != UserRole.Parent)
            throw new InvalidOperationException("Only parents have an invitation code.");
        InvitationCode = GenerateInvitationCode();
        SetUpdatedAt();
    }

    private static string GenerateInvitationCode()
    {
        // Format: PAR-XXXXXX (6 chars, no ambiguous like 0/O, 1/I)
        const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
        var code = new string(Enumerable.Range(0, 6)
            .Select(_ => chars[RandomNumberGenerator.GetInt32(chars.Length)]).ToArray());
        return $"PAR-{code}";
    }

    public string FullName => $"{FirstName} {LastName}";
}

