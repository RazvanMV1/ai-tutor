using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Application.Features.Classrooms.Commands.CreateClassroomLesson;
using AiTutor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Classrooms.Queries.GetClassroomLessons;

public class GetClassroomLessonsQueryHandler
    : IRequestHandler<GetClassroomLessonsQuery, Result<List<ClassroomLessonResponse>>>
{
    private readonly IApplicationDbContext _context;

    public GetClassroomLessonsQueryHandler(IApplicationDbContext context)
        => _context = context;

    public async Task<Result<List<ClassroomLessonResponse>>> Handle(
        GetClassroomLessonsQuery request, CancellationToken ct)
    {
        var classroom = await _context.Classrooms
            .FirstOrDefaultAsync(c => c.Id == request.ClassroomId, ct);
        if (classroom is null)
            throw new NotFoundException(nameof(Classroom), request.ClassroomId);

        var isTeacher = classroom.TeacherId == request.UserId;
        var isStudent = await _context.ClassroomMembers
            .AnyAsync(m => m.ClassroomId == request.ClassroomId &&
                          m.StudentId == request.UserId && m.IsActive, ct);
        if (!isTeacher && !isStudent)
            throw new ForbiddenAccessException();

        var lessons = await _context.ClassroomLessons
            .Where(l => l.ClassroomId == request.ClassroomId)
            .OrderBy(l => l.OrderIndex)
            .ToListAsync(ct);

        var result = lessons.Select(l => new ClassroomLessonResponse(
            l.Id, l.ClassroomId, l.Title, l.Content,
            l.OrderIndex, l.Difficulty, l.CreatedAt
        )).ToList();

        return Result<List<ClassroomLessonResponse>>.Success(result);
    }
}
