using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Users.Queries.GetInvitationCode;

public class GetInvitationCodeQueryHandler : IRequestHandler<GetInvitationCodeQuery, Result<InvitationCodeDto>>
{
    private readonly IApplicationDbContext _context;

    public GetInvitationCodeQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<InvitationCodeDto>> Handle(GetInvitationCodeQuery request, CancellationToken cancellationToken)
    {
        var parent = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.ParentId, cancellationToken);

        if (parent is null)
            return Result<InvitationCodeDto>.Failure("Părintele nu a fost găsit.", 404);

        if (parent.Role != UserRole.Parent)
            return Result<InvitationCodeDto>.Failure("Doar părinții au cod de invitație.", 400);

        // Backfill if missing (for users created before this feature)
        if (string.IsNullOrEmpty(parent.InvitationCode))
        {
            parent.RegenerateInvitationCode();
            await _context.SaveChangesAsync(cancellationToken);
        }

        return Result<InvitationCodeDto>.Success(
            new InvitationCodeDto(parent.Id, parent.InvitationCode!));
    }
}
