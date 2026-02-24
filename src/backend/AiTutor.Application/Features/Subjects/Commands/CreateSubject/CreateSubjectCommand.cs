using AiTutor.Application.Common.Models;
using AiTutor.Domain.Enums;
using MediatR;

namespace AiTutor.Application.Features.Subjects.Commands.CreateSubject;

public record CreateSubjectCommand(
    string Name,
    string Description,
    SubjectType Type) : IRequest<Result<SubjectResponse>>;

public record SubjectResponse(Guid Id, string Name, SubjectType Type);
