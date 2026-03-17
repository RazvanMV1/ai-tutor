using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Classrooms.Commands.AddMember;

public class AddMemberCommandHandler : IRequestHandler<AddMemberCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;

    public AddMemberCommandHandler(IApplicationDbContext context)
        => _context = context;

    public async Task<Result<bool>> Handle(AddMemberCommand request, CancellationToken ct)
    {
        var classroom = await _context.Classrooms
            .Include(c => c.Members)
            .FirstOrDefaultAsync(c => c.Id == request.ClassroomId, ct);
        if (classroom is null)
            throw new NotFoundException(nameof(Classroom), request.ClassroomId);
        if (classroom.TeacherId != request.TeacherId)
            throw new ForbiddenAccessException();

        var student = await _context.Users
            .FirstOrDefaultAsync(u => EF.Property<string>(u, "Email") == request.StudentEmail.ToLowerInvariant(), ct);
        if (student is null)
            return Result<bool>.Failure("Nu există niciun utilizator cu acest email.", 404);

        var activeMembers = classroom.Members.Count(m => m.IsActive);
        if (activeMembers >= classroom.MaxStudents)
            return Result<bool>.Failure("Clasa este plină (maxim 50 elevi).", 400);

        var alreadyMember = classroom.Members
            .Any(m => m.StudentId == student.Id && m.IsActive);
        if (alreadyMember)
            return Result<bool>.Failure("Elevul este deja în această clasă.", 400);

        var member = ClassroomMember.Create(classroom.Id, student.Id);
        _context.ClassroomMembers.Add(member);
        await _context.SaveChangesAsync(ct);

        return Result<bool>.Success(true);
    }
}
