using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
{
    private readonly IApplicationDbContext _context;

    public GetUserByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<UserDto>> Handle(GetUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user is null)
            throw new NotFoundException(nameof(user), request.UserId);

        return Result<UserDto>.Success(new UserDto(
            user.Id,
            user.FullName,
            user.Email.Value,
            user.Role,
            user.IsActive,
            user.CreatedAt));
    }
}
