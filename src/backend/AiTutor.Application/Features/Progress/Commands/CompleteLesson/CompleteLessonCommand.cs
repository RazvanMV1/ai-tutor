using AiTutor.Application.Common.Models;
using MediatR;

namespace AiTutor.Application.Features.Progress.Commands.CompleteLesson;

public record CompleteLessonCommand(
    Guid UserId,
    Guid LessonId,
    int ScorePercentage) : IRequest<Result<ProgressResponse>>;

public record ProgressResponse(Guid Id, bool IsCompleted, int ScorePercentage, DateTime? CompletedAt);
