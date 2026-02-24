using AiTutor.Application.Common.Models;
using AiTutor.Domain.Enums;
using MediatR;

namespace AiTutor.Application.Features.Lessons.Queries.GetLessonById;

public record GetLessonByIdQuery(Guid LessonId) : IRequest<Result<LessonDto>>;

public record LessonDto(
    Guid Id,
    string Title,
    string Content,
    int OrderIndex,
    DifficultyLevel Difficulty,
    Guid SubjectId,
    DateTime CreatedAt);
