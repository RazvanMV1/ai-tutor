namespace AiTutor.Web.Models;

public record SubjectDto(
    Guid Id,
    string Name,
    int Type,
    string? Description = null);

public record CreateSubjectRequest(
    string Name,
    string Description,
    int Type);

public record LessonDto(
    Guid Id,
    string Title,
    int Difficulty,
    Guid SubjectId,
    string? Content = null,
    int OrderIndex = 0,
    DateTime? CreatedAt = null);

public record CreateLessonRequest(
    string Title,
    string Content,
    int OrderIndex,
    int Difficulty,
    Guid SubjectId);
