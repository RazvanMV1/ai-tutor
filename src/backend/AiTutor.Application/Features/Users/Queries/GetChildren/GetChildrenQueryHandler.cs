using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Users.Queries.GetChildren;

public class GetChildrenQueryHandler : IRequestHandler<GetChildrenQuery, Result<List<ChildDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetChildrenQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<ChildDto>>> Handle(GetChildrenQuery request, CancellationToken cancellationToken)
    {
        var children = await _context.Users
            .Where(u => u.ParentId == request.ParentId)
            .OrderBy(u => u.FirstName)
            .Select(u => new ChildDto(
                u.Id,
                u.FirstName + " " + u.LastName,
                u.Email.Value,
                u.Role,
                u.CreatedAt))
            .ToListAsync(cancellationToken);

        return Result<List<ChildDto>>.Success(children);
    }
}
