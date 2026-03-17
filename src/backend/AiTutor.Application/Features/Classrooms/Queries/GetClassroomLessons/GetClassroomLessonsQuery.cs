using AiTutor.Application.Common.Models;
using AiTutor.Application.Features.Classrooms.Commands.CreateClassroomLesson;
using MediatR;

namespace AiTutor.Application.Features.Classrooms.Queries.GetClassroomLessons;

public record GetClassroomLessonsQuery(Guid ClassroomId, Guid UserId)
    : IRequest<Result<List<ClassroomLessonResponse>>>;
