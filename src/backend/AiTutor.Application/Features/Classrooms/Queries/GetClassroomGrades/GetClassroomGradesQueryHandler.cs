using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Application.Features.Classrooms.Commands.AddGrade;
using AiTutor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Classrooms.Queries.GetClassroomGrades;

public class GetClassroomGradesQueryHandler
    : IRequestHandler<GetClassroomGradesQuery, Result<List<GradeResponse>>>
{
    private readonly IApplicationDbContext _context;

    public GetClassroomGradesQueryHandler(IApplicationDbContext context)
        => _context = context;

    public async Task<Result<List<GradeResponse>>> Handle(
        GetClassroomGradesQuery request, CancellationToken ct)
    {
        var classroom = await _context.Classrooms
            .FirstOrDefaultAsync(c => c.Id == request.ClassroomId, ct);
        if (classroom is null)
            throw new NotFoundException(nameof(Classroom), request.ClassroomId);

        IQueryable<Grade> query = _context.Grades
            .Include(g => g.Student)
            .Where(g => g.ClassroomId == request.ClassroomId);

        if (!request.IsTeacher)
            query = query.Where(g => g.StudentId == request.UserId);

        var grades = await query
            .OrderByDescending(g => g.GradedAt)
            .ToListAsync(ct);

        var result = grades.Select(g => new GradeResponse(
            g.Id, g.ClassroomId, g.StudentId,
            g.Student.FullName, g.TeacherId,
            g.Value, g.Description, g.GradedAt
        )).ToList();

        return Result<List<GradeResponse>>.Success(result);
    }
}
