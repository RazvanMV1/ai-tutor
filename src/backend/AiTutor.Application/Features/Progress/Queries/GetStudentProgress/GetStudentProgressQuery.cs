using AiTutor.Application.Common.Models;
using MediatR;

namespace AiTutor.Application.Features.Progress.Queries.GetStudentProgress;

public record GetStudentProgressQuery(Guid UserId) : IRequest<Result<List<StudentProgressDto>>>;

public record StudentProgressDto(
    Guid LessonId,
    string LessonTitle,
    bool IsCompleted,
    int ScorePercentage,
    int AttemptsCount,
    DateTime? CompletedAt);
