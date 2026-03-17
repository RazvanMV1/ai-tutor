using AiTutor.Application.Common.Models;
using AiTutor.Domain.Enums;
using MediatR;

namespace AiTutor.Application.Features.Classrooms.Commands.CreateClassroomLesson;

public record CreateClassroomLessonCommand(
    Guid ClassroomId,
    Guid TeacherId,
    string Title,
    string Content,
    int OrderIndex,
    DifficultyLevel Difficulty
) : IRequest<Result<ClassroomLessonResponse>>;

public record ClassroomLessonResponse(
    Guid Id,
    Guid ClassroomId,
    string Title,
    string Content,
    int OrderIndex,
    DifficultyLevel Difficulty,
    DateTime CreatedAt
);
