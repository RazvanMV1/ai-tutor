using AiTutor.Application.Common.Models;
using MediatR;

namespace AiTutor.Application.Features.Quizzes.Commands.SubmitQuiz;

public record SubmitQuizCommand(
    Guid QuizId,
    Guid UserId,
    List<QuizAnswerDto> Answers) : IRequest<Result<QuizResultResponse>>;

public record QuizAnswerDto(Guid QuestionId, string Answer);

public record QuizResultResponse(
    Guid QuizId,
    Guid UserId,
    int TotalQuestions,
    int CorrectAnswers,
    int ScorePercentage,
    List<QuestionResultDto> Results);

public record QuestionResultDto(
    Guid QuestionId,
    string QuestionText,
    string UserAnswer,
    string CorrectAnswer,
    bool IsCorrect,
    string? Explanation);
