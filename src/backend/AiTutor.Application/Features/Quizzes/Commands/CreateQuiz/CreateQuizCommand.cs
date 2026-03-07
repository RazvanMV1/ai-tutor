using AiTutor.Application.Common.Models;
using AiTutor.Domain.Enums;
using MediatR;

namespace AiTutor.Application.Features.Quizzes.Commands.CreateQuiz;

public record CreateQuizCommand(
    string Title,
    DifficultyLevel Difficulty,
    Guid LessonId,
    int TimeLimitMinutes) : IRequest<Result<QuizResponse>>;

public record QuizResponse(
    Guid Id,
    string Title,
    DifficultyLevel Difficulty,
    Guid LessonId,
    int TimeLimitMinutes);
