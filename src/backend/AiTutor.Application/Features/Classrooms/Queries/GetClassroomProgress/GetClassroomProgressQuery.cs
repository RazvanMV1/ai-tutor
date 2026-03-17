using AiTutor.Application.Common.Models;
using MediatR;

namespace AiTutor.Application.Features.Classrooms.Queries.GetClassroomProgress;

public record ClassroomProgressResponse(
    Guid StudentId,
    string StudentName,
    Guid ClassroomLessonId,
    string LessonTitle,
    bool IsCompleted,
    double ScorePercentage,
    int AttemptsCount,
    DateTime? CompletedAt
);

public record GetClassroomProgressQuery(Guid ClassroomId, Guid UserId) : IRequest<Result<List<ClassroomProgressResponse>>>;
