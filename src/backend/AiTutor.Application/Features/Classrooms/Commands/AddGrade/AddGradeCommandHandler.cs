using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Classrooms.Commands.AddGrade;

public class AddGradeCommandHandler : IRequestHandler<AddGradeCommand, Result<GradeResponse>>
{
    private readonly IApplicationDbContext _context;

    public AddGradeCommandHandler(IApplicationDbContext context)
        => _context = context;

    public async Task<Result<GradeResponse>> Handle(AddGradeCommand request, CancellationToken ct)
    {
        var classroom = await _context.Classrooms
            .FirstOrDefaultAsync(c => c.Id == request.ClassroomId, ct);
        if (classroom is null)
            throw new NotFoundException(nameof(Classroom), request.ClassroomId);
        if (classroom.TeacherId != request.TeacherId)
            throw new ForbiddenAccessException();

        var student = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.StudentId, ct);
        if (student is null)
            throw new NotFoundException(nameof(User), request.StudentId);

        var isMember = await _context.ClassroomMembers
            .AnyAsync(m => m.ClassroomId == request.ClassroomId &&
                          m.StudentId == request.StudentId && m.IsActive, ct);
        if (!isMember)
            return Result<GradeResponse>.Failure("Elevul nu este în această clasă.", 400);

        var grade = Grade.Create(
            request.ClassroomId,
            request.StudentId,
            request.TeacherId,
            request.Value,
            request.Description
        );

        _context.Grades.Add(grade);
        await _context.SaveChangesAsync(ct);

        return Result<GradeResponse>.Success(new GradeResponse(
            grade.Id, grade.ClassroomId, grade.StudentId,
            student.FullName, grade.TeacherId,
            grade.Value, grade.Description, grade.GradedAt
        ), 201);
    }
}
