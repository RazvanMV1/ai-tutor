using AiTutor.Application.Common.Models;
using MediatR;

namespace AiTutor.Application.Features.Classrooms.Commands.SubmitClassroomQuiz;

public record QuizAnswerDto(Guid QuestionId, string Answer);

public record SubmitClassroomQuizCommand(
    Guid ClassroomId,
    Guid QuizId,
    Guid StudentId,
    List<QuizAnswerDto> Answers
) : IRequest<Result<ClassroomQuizResultResponse>>;

public record QuizQuestionResult(
    Guid QuestionId,
    string QuestionText,
    string UserAnswer,
    string CorrectAnswer,
    bool IsCorrect,
    string? Explanation
);

public record ClassroomQuizResultResponse(
    Guid QuizId,
    Guid StudentId,
    int TotalQuestions,
    int CorrectAnswers,
    double ScorePercentage,
    List<QuizQuestionResult> Results
);
