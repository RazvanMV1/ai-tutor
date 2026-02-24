using AiTutor.Application.Common.Models;
using AiTutor.Domain.Enums;
using MediatR;

namespace AiTutor.Application.Features.Users.Commands.CreateUser;

public record CreateUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    UserRole Role) : IRequest<Result<CreateUserResponse>>;

public record CreateUserResponse(Guid Id, string FullName, string Email, UserRole Role);
