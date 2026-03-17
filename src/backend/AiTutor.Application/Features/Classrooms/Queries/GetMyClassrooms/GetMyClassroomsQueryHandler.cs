using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Application.Features.Classrooms.Commands.CreateClassroom;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Classrooms.Queries.GetMyClassrooms;

public class GetMyClassroomsQueryHandler
    : IRequestHandler<GetMyClassroomsQuery, Result<List<ClassroomResponse>>>
{
    private readonly IApplicationDbContext _context;

    public GetMyClassroomsQueryHandler(IApplicationDbContext context)
        => _context = context;

    public async Task<Result<List<ClassroomResponse>>> Handle(
        GetMyClassroomsQuery request, CancellationToken ct)
    {
        // Clasele ca profesor
        var asTeacher = await _context.Classrooms
            .Where(c => c.TeacherId == request.UserId && c.IsActive)
            .ToListAsync(ct);

        // Clasele ca elev
        var asStudent = await _context.ClassroomMembers
            .Include(m => m.Classroom)
            .Where(m => m.StudentId == request.UserId && m.IsActive && m.Classroom.IsActive)
            .Select(m => m.Classroom)
            .ToListAsync(ct);

        var allClassrooms = asTeacher.Union(asStudent).Distinct().ToList();

        var response = new List<ClassroomResponse>();
        foreach (var c in allClassrooms)
        {
            var memberCount = await _context.ClassroomMembers
                .CountAsync(m => m.ClassroomId == c.Id && m.IsActive, ct);
            var lessonCount = await _context.ClassroomLessons
                .CountAsync(l => l.ClassroomId == c.Id, ct);
            response.Add(new ClassroomResponse(
                c.Id, c.Name, c.Description, c.SubjectType,
                c.TeacherId, c.ClassCode, c.MaxStudents,
                c.IsActive, memberCount, lessonCount
            ));
        }

        return Result<List<ClassroomResponse>>.Success(response);
    }
}
