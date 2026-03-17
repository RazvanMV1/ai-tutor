using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Classrooms.Commands.JoinClassroom;

public class JoinClassroomCommandHandler : IRequestHandler<JoinClassroomCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;

    public JoinClassroomCommandHandler(IApplicationDbContext context)
        => _context = context;

    public async Task<Result<bool>> Handle(JoinClassroomCommand request, CancellationToken ct)
    {
        var classroom = await _context.Classrooms
            .Include(c => c.Members)
            .FirstOrDefaultAsync(c => c.ClassCode == request.ClassCode && c.IsActive, ct);
        if (classroom is null)
            return Result<bool>.Failure("Codul clasei este invalid sau clasa nu mai este activă.", 404);

        var activeMembers = classroom.Members.Count(m => m.IsActive);
        if (activeMembers >= classroom.MaxStudents)
            return Result<bool>.Failure("Clasa este plină (maxim 50 elevi).", 400);

        var alreadyMember = classroom.Members
            .Any(m => m.StudentId == request.StudentId && m.IsActive);
        if (alreadyMember)
            return Result<bool>.Failure("Ești deja înscris în această clasă.", 400);

        var member = ClassroomMember.Create(classroom.Id, request.StudentId);
        _context.ClassroomMembers.Add(member);
        await _context.SaveChangesAsync(ct);

        return Result<bool>.Success(true);
    }
}
