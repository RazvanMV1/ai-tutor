using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Users.Commands.LinkParent;

public class LinkParentCommandHandler : IRequestHandler<LinkParentCommand, Result<LinkParentResponse>>
{
    private readonly IApplicationDbContext _context;

    public LinkParentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<LinkParentResponse>> Handle(LinkParentCommand request, CancellationToken cancellationToken)
    {
        var student = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.StudentId, cancellationToken);

        if (student is null)
            return Result<LinkParentResponse>.Failure("Studentul nu a fost găsit.", 404);

        if (student.Role != UserRole.Student)
            return Result<LinkParentResponse>.Failure("Doar elevii pot fi legați de un părinte.", 400);

        var code = request.InvitationCode?.Trim().ToUpperInvariant();
        if (string.IsNullOrWhiteSpace(code))
            return Result<LinkParentResponse>.Failure("Codul de invitație este obligatoriu.", 400);

        var parent = await _context.Users
            .FirstOrDefaultAsync(u => u.InvitationCode == code && u.Role == UserRole.Parent, cancellationToken);

        if (parent is null)
            return Result<LinkParentResponse>.Failure("Cod de invitație invalid.", 404);

        student.LinkToParent(parent.Id);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<LinkParentResponse>.Success(
            new LinkParentResponse(parent.Id, parent.FullName), 200);
    }
}
