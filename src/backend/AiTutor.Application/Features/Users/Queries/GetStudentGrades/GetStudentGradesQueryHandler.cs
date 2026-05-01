using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Users.Queries.GetStudentGrades;

public class GetStudentGradesQueryHandler
    : IRequestHandler<GetStudentGradesQuery, Result<List<StudentGradeDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetStudentGradesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<StudentGradeDto>>> Handle(GetStudentGradesQuery request, CancellationToken cancellationToken)
    {
        var student = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.StudentId, cancellationToken);

        if (student is null)
            return Result<List<StudentGradeDto>>.Failure("Studentul nu a fost găsit.", 404);

        // Authorization: requester must be the student or his parent
        var isStudentSelf = request.RequesterId == request.StudentId;
        var isParent = student.ParentId.HasValue && student.ParentId.Value == request.RequesterId;

        if (!isStudentSelf && !isParent)
            return Result<List<StudentGradeDto>>.Failure("Nu ai permisiunea de a vedea aceste note.", 403);

        var raw = await (
            from g in _context.Grades
            where g.StudentId == request.StudentId
            join c in _context.Classrooms on g.ClassroomId equals c.Id
            join t in _context.Users on g.TeacherId equals t.Id
            orderby g.GradedAt descending
            select new
            {
                g.Id,
                g.Value,
                g.Description,
                g.GradedAt,
                g.ClassroomId,
                ClassroomName = c.Name,
                c.SubjectType,
                g.TeacherId,
                TeacherName = t.FirstName + " " + t.LastName
            }).ToListAsync(cancellationToken);

        var grades = raw.Select(x => new StudentGradeDto(
            x.Id,
            x.Value,
            x.Description,
            x.GradedAt,
            x.ClassroomId,
            x.ClassroomName,
            SubjectName(x.SubjectType),
            x.TeacherId,
            x.TeacherName)).ToList();

        return Result<List<StudentGradeDto>>.Success(grades);
    }

    private static string SubjectName(SubjectType type) => type switch
    {
        SubjectType.Mathematics => "Matematică",
        SubjectType.Romanian => "Română",
        SubjectType.Informatics => "Informatică",
        _ => "Altele"
    };
}
