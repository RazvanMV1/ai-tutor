using AiTutor.Application.Common.Models;
using MediatR;

namespace AiTutor.Application.Features.Classrooms.Commands.DeleteClassroomQuiz;

public record DeleteClassroomQuizCommand(
    Guid ClassroomId,
    Guid QuizId,
    Guid TeacherId
) : IRequest<Result<bool>>;
