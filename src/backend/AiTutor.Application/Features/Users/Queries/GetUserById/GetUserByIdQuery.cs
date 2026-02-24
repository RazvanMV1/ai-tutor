using AiTutor.Application.Common.Models;
using AiTutor.Domain.Enums;
using MediatR;

namespace AiTutor.Application.Features.Users.Queries.GetUserById;

public record GetUserByIdQuery(Guid UserId) : IRequest<Result<UserDto>>;

public record UserDto(
    Guid Id,
    string FullName,
    string Email,
    UserRole Role,
    bool IsActive,
    DateTime CreatedAt);
