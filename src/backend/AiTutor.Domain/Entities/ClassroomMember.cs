using AiTutor.Domain.Common;

namespace AiTutor.Domain.Entities;

public class ClassroomMember : BaseEntity
{
    public Guid ClassroomId { get; private set; }
    public Classroom Classroom { get; private set; } = null!;
    public Guid StudentId { get; private set; }
    public User Student { get; private set; } = null!;
    public DateTime JoinedAt { get; private set; }
    public bool IsActive { get; private set; } = true;

    private ClassroomMember() { }

    public static ClassroomMember Create(Guid classroomId, Guid studentId)
    {
        return new ClassroomMember
        {
            ClassroomId = classroomId,
            StudentId = studentId,
            JoinedAt = DateTime.UtcNow,
            IsActive = true
        };
    }

    public void Deactivate()
    {
        IsActive = false;
        SetUpdatedAt();
    }
}
