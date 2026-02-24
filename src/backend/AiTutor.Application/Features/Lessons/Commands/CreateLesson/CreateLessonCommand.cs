using AiTutor.Application.Common.Models;
using AiTutor.Domain.Enums;
using MediatR;

namespace AiTutor.Application.Features.Lessons.Commands.CreateLesson;

public record CreateLessonCommand(
    string Title,
    string Content,
    int OrderIndex,
    DifficultyLevel Difficulty,
    Guid SubjectId) : IRequest<Result<LessonResponse>>;

public record LessonResponse(Guid Id, string Title, DifficultyLevel Difficulty, Guid SubjectId);
