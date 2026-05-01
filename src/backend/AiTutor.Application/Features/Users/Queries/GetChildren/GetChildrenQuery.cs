using AiTutor.Application.Common.Models;
using AiTutor.Domain.Enums;
using MediatR;

namespace AiTutor.Application.Features.Users.Queries.GetChildren;

public record GetChildrenQuery(Guid ParentId) : IRequest<Result<List<ChildDto>>>;

public record ChildDto(
    Guid Id,
    string FullName,
    string Email,
    UserRole Role,
    DateTime CreatedAt);
