using AiTutor.Application.Common.Models;
using AiTutor.Application.Features.Classrooms.Commands.CreateClassroomLesson;
using AiTutor.Domain.Enums;
using MediatR;

namespace AiTutor.Application.Features.Classrooms.Commands.UpdateClassroomLesson;

public record UpdateClassroomLessonCommand(
    Guid ClassroomId,
    Guid LessonId,
    Guid TeacherId,
    string Title,
    string Content,
    DifficultyLevel Difficulty
) : IRequest<Result<ClassroomLessonResponse>>;
