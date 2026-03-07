namespace AiTutor.Web.Models;

public record SubjectDto(Guid Id, string Name, string Description, int Type);

public record LessonDto(
    Guid Id,
    string Title,
    string Content,
    int OrderIndex,
    int Difficulty,
    Guid SubjectId,
    DateTime CreatedAt);
