using AiTutor.Domain.Common;
using System.Security.Cryptography;
using AiTutor.Domain.Enums;
using AiTutor.Domain.Exceptions;

namespace AiTutor.Domain.Entities;

public class Classroom : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public SubjectType SubjectType { get; private set; }
    public Guid TeacherId { get; private set; }
    public User Teacher { get; private set; } = null!;
    public string ClassCode { get; private set; } = string.Empty;
    public int MaxStudents { get; private set; } = 50;
    public bool IsActive { get; private set; } = true;

    private readonly List<ClassroomMember> _members = new();
    public IReadOnlyCollection<ClassroomMember> Members => _members.AsReadOnly();

    private readonly List<ClassroomLesson> _lessons = new();
    public IReadOnlyCollection<ClassroomLesson> Lessons => _lessons.AsReadOnly();

    private Classroom() { }

    public static Classroom Create(string name, string description, SubjectType subjectType, Guid teacherId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Classroom name cannot be empty.");
        if (name.Length > 100)
            throw new DomainException("Classroom name cannot exceed 100 characters.");

        return new Classroom
        {
            Name = name,
            Description = description,
            SubjectType = subjectType,
            TeacherId = teacherId,
            ClassCode = GenerateClassCode(subjectType),
            MaxStudents = 50,
            IsActive = true
        };
    }

    public void Update(string name, string? description)
    {
        Name = name;
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }


    public void Deactivate()
    {
        IsActive = false;
        SetUpdatedAt();
    }

    public void RegenerateCode()
    {
        ClassCode = GenerateClassCode(SubjectType);
        SetUpdatedAt();
    }

    private static string GenerateClassCode(SubjectType subjectType)
    {
        var prefix = subjectType switch
        {
            SubjectType.Mathematics => "MAT",
            SubjectType.Romanian => "ROM",
            SubjectType.Informatics => "INF",
            _ => "GEN"
        };
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var suffix = new string(Enumerable.Range(0, 4)
            .Select(_ => chars[RandomNumberGenerator.GetInt32(chars.Length)]).ToArray());
        return $"{prefix}-{suffix}";
    }
}

