using AiTutor.Application.Common.Models;
using MediatR;

namespace AiTutor.Application.Features.Classrooms.Commands.DeleteClassroomLesson;

public record DeleteClassroomLessonCommand(
    Guid ClassroomId,
    Guid LessonId,
    Guid TeacherId
) : IRequest<Result<bool>>;
