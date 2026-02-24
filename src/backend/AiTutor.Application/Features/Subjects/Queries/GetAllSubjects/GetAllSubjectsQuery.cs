using AiTutor.Application.Common.Models;
using AiTutor.Domain.Enums;
using MediatR;

namespace AiTutor.Application.Features.Subjects.Queries.GetAllSubjects;

public record GetAllSubjectsQuery : IRequest<Result<List<SubjectDto>>>;

public record SubjectDto(Guid Id, string Name, string Description, SubjectType Type);
