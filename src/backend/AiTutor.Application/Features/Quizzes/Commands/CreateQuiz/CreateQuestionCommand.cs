using AiTutor.Application.Common.Models;
using MediatR;

namespace AiTutor.Application.Features.Questions.Commands.CreateQuestion;

public record CreateQuestionCommand(
    string Text,
    string CorrectAnswer,
    List<string> Options,
    int Points,
    Guid QuizId,
    string? Explanation = null) : IRequest<Result<QuestionResponse>>;

public record QuestionResponse(
    Guid Id,
    string Text,
    List<string> Options,
    int Points,
    Guid QuizId);
