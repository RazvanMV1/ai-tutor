using AiTutor.Application.Common.Models;
using AiTutor.Application.Features.Classrooms.Commands.CreateClassroomQuiz;
using AiTutor.Domain.Enums;
using MediatR;

namespace AiTutor.Application.Features.Classrooms.Commands.UpdateClassroomQuiz;

public record UpdateClassroomQuizCommand(
    Guid ClassroomId,
    Guid QuizId,
    Guid TeacherId,
    string Title,
    DifficultyLevel Difficulty,
    int TimeLimitMinutes
) : IRequest<Result<ClassroomQuizResponse>>;
