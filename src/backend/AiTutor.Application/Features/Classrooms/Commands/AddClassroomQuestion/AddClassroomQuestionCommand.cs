using AiTutor.Application.Common.Models;
using MediatR;

namespace AiTutor.Application.Features.Classrooms.Commands.AddClassroomQuestion;

public record AddClassroomQuestionCommand(
    Guid ClassroomId,
    Guid QuizId,
    Guid TeacherId,
    string Text,
    string CorrectAnswer,
    List<string> Options,
    int Points,
    string? Explanation
) : IRequest<Result<ClassroomQuestionResponse>>;

public record ClassroomQuestionResponse(
    Guid Id,
    Guid ClassroomQuizId,
    string Text,
    List<string> Options,
    int Points,
    DateTime CreatedAt
);
