using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Classrooms.Commands.RemoveMember;

public class RemoveMemberCommandHandler : IRequestHandler<RemoveMemberCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;

    public RemoveMemberCommandHandler(IApplicationDbContext context)
        => _context = context;

    public async Task<Result<bool>> Handle(RemoveMemberCommand request, CancellationToken ct)
    {
        var classroom = await _context.Classrooms
            .FirstOrDefaultAsync(c => c.Id == request.ClassroomId, ct);
        if (classroom is null)
            throw new NotFoundException(nameof(Classroom), request.ClassroomId);
        if (classroom.TeacherId != request.TeacherId)
            throw new ForbiddenAccessException();

        var member = await _context.ClassroomMembers
            .FirstOrDefaultAsync(m => m.ClassroomId == request.ClassroomId &&
                                     m.StudentId == request.StudentId && m.IsActive, ct);
        if (member is null)
            return Result<bool>.Failure("Elevul nu este în această clasă.", 404);

        member.Deactivate();

        // Șterge progresul și notele
        var progresses = await _context.ClassroomProgresses
            .Where(p => p.ClassroomId == request.ClassroomId && p.StudentId == request.StudentId)
            .ToListAsync(ct);
        _context.ClassroomProgresses.RemoveRange(progresses);

        var grades = await _context.Grades
            .Where(g => g.ClassroomId == request.ClassroomId && g.StudentId == request.StudentId)
            .ToListAsync(ct);
        _context.Grades.RemoveRange(grades);

        await _context.SaveChangesAsync(ct);
        return Result<bool>.Success(true);
    }
}
