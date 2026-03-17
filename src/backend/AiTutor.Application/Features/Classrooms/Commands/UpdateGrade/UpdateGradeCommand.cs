using AiTutor.Application.Common.Models;
using AiTutor.Application.Features.Classrooms.Commands.AddGrade;
using MediatR;

namespace AiTutor.Application.Features.Classrooms.Commands.UpdateGrade;

public record UpdateGradeCommand(
    Guid GradeId,
    Guid TeacherId,
    int Value,
    string Description
) : IRequest<Result<GradeResponse>>;
