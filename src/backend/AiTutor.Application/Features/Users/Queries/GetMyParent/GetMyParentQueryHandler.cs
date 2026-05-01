using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Users.Queries.GetMyParent;

public class GetMyParentQueryHandler : IRequestHandler<GetMyParentQuery, Result<ParentInfoDto?>>
{
    private readonly IApplicationDbContext _context;

    public GetMyParentQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<ParentInfoDto?>> Handle(GetMyParentQuery request, CancellationToken cancellationToken)
    {
        var student = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.StudentId, cancellationToken);

        if (student is null)
            return Result<ParentInfoDto?>.Failure("Studentul nu a fost găsit.", 404);

        if (student.ParentId is null)
            return Result<ParentInfoDto?>.Success(null);

        var parent = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == student.ParentId.Value && u.Role == UserRole.Parent, cancellationToken);

        if (parent is null)
            return Result<ParentInfoDto?>.Success(null);

        return Result<ParentInfoDto?>.Success(new ParentInfoDto(
            parent.Id,
            parent.FullName,
            parent.Email.Value));
    }
}
