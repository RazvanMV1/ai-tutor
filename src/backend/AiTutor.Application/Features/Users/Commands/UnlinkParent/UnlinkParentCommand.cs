using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Users.Commands.UnlinkParent;

public record UnlinkParentCommand(Guid StudentId) : IRequest<Result<bool>>;

public class UnlinkParentCommandHandler : IRequestHandler<UnlinkParentCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;

    public UnlinkParentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<bool>> Handle(UnlinkParentCommand request, CancellationToken cancellationToken)
    {
        var student = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.StudentId, cancellationToken);

        if (student is null)
            return Result<bool>.Failure("Studentul nu a fost găsit.", 404);

        if (student.Role != UserRole.Student)
            return Result<bool>.Failure("Doar elevii pot fi deconectați.", 400);

        if (student.ParentId is null)
            return Result<bool>.Failure("Nu ești conectat la niciun părinte.", 400);

        student.UnlinkFromParent();
        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
