using AiTutor.Application.Common.Models;
using MediatR;

namespace AiTutor.Application.Features.Classrooms.Commands.AddGrade;

public record AddGradeCommand(
    Guid ClassroomId,
    Guid StudentId,
    Guid TeacherId,
    int Value,
    string Description
) : IRequest<Result<GradeResponse>>;

public record GradeResponse(
    Guid Id,
    Guid ClassroomId,
    Guid StudentId,
    string StudentName,
    Guid TeacherId,
    int Value,
    string Description,
    DateTime GradedAt
);
