using AiTutor.Application.Common.Models;
using MediatR;

namespace AiTutor.Application.Features.Classrooms.Commands.DeleteGrade;

public record DeleteGradeCommand(Guid GradeId, Guid TeacherId) : IRequest<Result<bool>>;
