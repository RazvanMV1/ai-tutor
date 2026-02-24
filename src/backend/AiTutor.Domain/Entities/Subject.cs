using AiTutor.Domain.Common;
using AiTutor.Domain.Enums;

namespace AiTutor.Domain.Entities;

public class Subject : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public SubjectType Type { get; private set; }

    private readonly List<Lesson> _lessons = new();
    public IReadOnlyCollection<Lesson> Lessons => _lessons.AsReadOnly();

    private Subject() { }

    public static Subject Create(string name, string description, SubjectType type)
    {
        return new Subject
        {
            Name = name,
            Description = description,
            Type = type
        };
    }

    public void Update(string name, string description)
    {
        Name = name;
        Description = description;
        SetUpdatedAt();
    }
}
