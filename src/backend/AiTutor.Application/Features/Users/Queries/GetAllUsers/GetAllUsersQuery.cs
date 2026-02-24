using AiTutor.Application.Common.Models;
using AiTutor.Application.Features.Users.Queries.GetUserById;
using MediatR;

namespace AiTutor.Application.Features.Users.Queries.GetAllUsers;

public record GetAllUsersQuery(int PageNumber = 1, int PageSize = 10)
    : IRequest<Result<PaginatedList<UserDto>>>;
