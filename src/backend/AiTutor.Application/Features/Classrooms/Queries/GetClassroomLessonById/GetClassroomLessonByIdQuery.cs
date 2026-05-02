using AiTutor.Application.Common.Models;
using AiTutor.Application.Features.Classrooms.Commands.CreateClassroomLesson;
using MediatR;

namespace AiTutor.Application.Features.Classrooms.Queries.GetClassroomLessonById;

public record GetClassroomLessonByIdQuery(
    Guid ClassroomId,
    Guid LessonId,
    Guid UserId
) : IRequest<Result<ClassroomLessonResponse>>;
