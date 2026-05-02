using AiTutor.Application.Common.Models;
using MediatR;

namespace AiTutor.Application.Features.Classrooms.Commands.DeleteClassroomQuestion;

public record DeleteClassroomQuestionCommand(
    Guid ClassroomId,
    Guid QuizId,
    Guid QuestionId,
    Guid TeacherId
) : IRequest<Result<bool>>;
