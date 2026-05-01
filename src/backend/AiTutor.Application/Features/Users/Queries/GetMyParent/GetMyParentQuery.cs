using AiTutor.Application.Common.Models;
using MediatR;

namespace AiTutor.Application.Features.Users.Queries.GetMyParent;

public record GetMyParentQuery(Guid StudentId) : IRequest<Result<ParentInfoDto?>>;

public record ParentInfoDto(
    Guid ParentId,
    string FullName,
    string Email);
