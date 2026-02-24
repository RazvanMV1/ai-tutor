using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Application.Features.Users.Queries.GetUserById;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<PaginatedList<UserDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetAllUsersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<PaginatedList<UserDto>>> Handle(GetAllUsersQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Users.AsNoTracking();

        var totalCount = await query.CountAsync(cancellationToken);

        var users = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(u => new UserDto(u.Id, u.FullName, u.Email.Value, u.Role, u.IsActive, u.CreatedAt))
            .ToListAsync(cancellationToken);

        return Result<PaginatedList<UserDto>>.Success(
            new PaginatedList<UserDto>(users, totalCount, request.PageNumber, request.PageSize));
    }
}
