using AiTutor.Application.Common.Models;
using AiTutor.Domain.Enums;
using MediatR;

namespace AiTutor.Application.Features.Quizzes.Queries.GetQuizzesByLesson;

public record GetQuizzesByLessonQuery(Guid LessonId) : IRequest<Result<List<QuizSummaryDto>>>;

public record QuizSummaryDto(
    Guid Id,
    string Title,
    DifficultyLevel Difficulty,
    int TimeLimitMinutes,
    int QuestionsCount);
