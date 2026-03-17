using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Classrooms.Queries.GetClassroomProgress;

public class GetClassroomProgressQueryHandler : IRequestHandler<GetClassroomProgressQuery, Result<List<ClassroomProgressResponse>>>
{
    private readonly IApplicationDbContext _context;
    public GetClassroomProgressQueryHandler(IApplicationDbContext context) => _context = context;
    public async Task<Result<List<ClassroomProgressResponse>>> Handle(GetClassroomProgressQuery request, CancellationToken ct)
    {
        var classroom = await _context.Classrooms
            .FirstOrDefaultAsync(c => c.Id == request.ClassroomId, ct)
            ?? throw new NotFoundException("Classroom", request.ClassroomId);
        var isTeacher = classroom.TeacherId == request.UserId;
        var isMember = await _context.ClassroomMembers
            .AnyAsync(m => m.ClassroomId == request.ClassroomId && m.StudentId == request.UserId && m.IsActive, ct);
        if (!isTeacher && !isMember)
            throw new ForbiddenAccessException();
        var progressQuery = _context.ClassroomProgresses
            .Where(p => p.ClassroomId == request.ClassroomId);
        if (!isTeacher)
            progressQuery = progressQuery.Where(p => p.StudentId == request.UserId);
        var progresses = await progressQuery.ToListAsync(ct);
        var lessons = await _context.ClassroomLessons
            .Where(l => l.ClassroomId == request.ClassroomId)
            .ToDictionaryAsync(l => l.Id, l => l.Title, ct);
        var studentIds = progresses.Select(p => p.StudentId).Distinct().ToList();
        var users = await _context.Users
            .Where(u => studentIds.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id, u => u.FullName, ct);
        var response = progresses.Select(p => new ClassroomProgressResponse(
            p.StudentId,
            users.TryGetValue(p.StudentId, out var name) ? name : "Unknown",
            p.ClassroomLessonId,
            lessons.TryGetValue(p.ClassroomLessonId, out var title) ? title : "Unknown",
            p.IsCompleted, p.ScorePercentage, p.AttemptsCount, p.CompletedAt)).ToList();
        return Result<List<ClassroomProgressResponse>>.Success(response);
    }
}

