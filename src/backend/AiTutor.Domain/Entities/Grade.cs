using AiTutor.Domain.Common;
using AiTutor.Domain.Exceptions;

namespace AiTutor.Domain.Entities;

public class Grade : BaseEntity
{
    public Guid ClassroomId { get; private set; }
    public Classroom Classroom { get; private set; } = null!;
    public Guid StudentId { get; private set; }
    public User Student { get; private set; } = null!;
    public Guid TeacherId { get; private set; }
    public User Teacher { get; private set; } = null!;
    public int Value { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public DateTime GradedAt { get; private set; }

    private Grade() { }

    public static Grade Create(Guid classroomId, Guid studentId, Guid teacherId,
        int value, string description)
    {
        if (value < 1 || value > 10)
            throw new DomainException("Grade value must be between 1 and 10.");

        return new Grade
        {
            ClassroomId = classroomId,
            StudentId = studentId,
            TeacherId = teacherId,
            Value = value,
            Description = description,
            GradedAt = DateTime.UtcNow
        };
    }

    public void Update(int value, string description)
    {
        if (value < 1 || value > 10)
            throw new DomainException("Grade value must be between 1 and 10.");
        Value = value;
        Description = description;
        SetUpdatedAt();
    }
}
