using AiTutor.Application.Common.Models;
using AiTutor.Application.Features.Classrooms.Commands.CreateClassroomQuiz;
using MediatR;

namespace AiTutor.Application.Features.Classrooms.Queries.GetClassroomQuizzesByLesson;

public record GetClassroomQuizzesByLessonQuery(
    Guid ClassroomId,
    Guid LessonId,
    Guid UserId
) : IRequest<Result<List<ClassroomQuizResponse>>>;
