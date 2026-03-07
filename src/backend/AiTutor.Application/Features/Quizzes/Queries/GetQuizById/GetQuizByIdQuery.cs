using AiTutor.Application.Common.Models;
using AiTutor.Domain.Enums;
using MediatR;

namespace AiTutor.Application.Features.Quizzes.Queries.GetQuizById;

public record GetQuizByIdQuery(Guid QuizId) : IRequest<Result<QuizDetailDto>>;

public record QuizDetailDto(
    Guid Id,
    string Title,
    DifficultyLevel Difficulty,
    Guid LessonId,
    int TimeLimitMinutes,
    List<QuestionDto> Questions);

public record QuestionDto(
    Guid Id,
    string Text,
    List<string> Options,
    int Points);
