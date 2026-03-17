using AiTutor.Application.Common.Models;
using AiTutor.Domain.Enums;
using MediatR;

namespace AiTutor.Application.Features.Classrooms.Commands.CreateClassroomQuiz;

public record CreateClassroomQuizCommand(
    Guid ClassroomId,
    Guid LessonId,
    Guid TeacherId,
    string Title,
    DifficultyLevel Difficulty,
    int TimeLimitMinutes
) : IRequest<Result<ClassroomQuizResponse>>;

public record ClassroomQuizResponse(
    Guid Id,
    Guid ClassroomLessonId,
    Guid ClassroomId,
    string Title,
    DifficultyLevel Difficulty,
    int TimeLimitMinutes,
    int QuestionCount,
    DateTime CreatedAt
);
