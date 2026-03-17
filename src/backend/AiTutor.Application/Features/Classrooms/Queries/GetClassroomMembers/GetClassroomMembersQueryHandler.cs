using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Classrooms.Queries.GetClassroomMembers;

public class GetClassroomMembersQueryHandler
    : IRequestHandler<GetClassroomMembersQuery, Result<List<ClassroomMemberDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetClassroomMembersQueryHandler(IApplicationDbContext context)
        => _context = context;

    public async Task<Result<List<ClassroomMemberDto>>> Handle(
        GetClassroomMembersQuery request, CancellationToken ct)
    {
        var classroom = await _context.Classrooms
            .FirstOrDefaultAsync(c => c.Id == request.ClassroomId, ct);
        if (classroom is null)
            throw new NotFoundException(nameof(Classroom), request.ClassroomId);
        if (classroom.TeacherId != request.TeacherId)
            throw new ForbiddenAccessException();

        var members = await _context.ClassroomMembers
            .Include(m => m.Student)
            .Where(m => m.ClassroomId == request.ClassroomId && m.IsActive)
            .OrderBy(m => m.JoinedAt)
            .ToListAsync(ct);

        var result = members.Select(m => new ClassroomMemberDto(
            m.StudentId,
            m.Student.FullName,
            m.Student.Email.Value,
            m.JoinedAt
        )).ToList();

        return Result<List<ClassroomMemberDto>>.Success(result);
    }
}
