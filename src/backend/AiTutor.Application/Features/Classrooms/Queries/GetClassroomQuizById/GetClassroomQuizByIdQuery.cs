using AiTutor.Application.Common.Models;
using AiTutor.Domain.Enums;
using MediatR;

namespace AiTutor.Application.Features.Classrooms.Queries.GetClassroomQuizById;

public record GetClassroomQuizByIdQuery(
    Guid ClassroomId,
    Guid QuizId,
    Guid UserId
) : IRequest<Result<ClassroomQuizDetailResponse>>;

public record ClassroomQuizDetailResponse(
    Guid Id,
    Guid ClassroomLessonId,
    Guid ClassroomId,
    string Title,
    DifficultyLevel Difficulty,
    int TimeLimitMinutes,
    DateTime CreatedAt,
    List<ClassroomQuestionDetailResponse> Questions
);

public record ClassroomQuestionDetailResponse(
    Guid Id,
    string Text,
    string CorrectAnswer,
    List<string> Options,
    int Points,
    string? Explanation
);
