using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Application.Features.Classrooms.Commands.AddGrade;
using AiTutor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Classrooms.Commands.UpdateGrade;

public class UpdateGradeCommandHandler : IRequestHandler<UpdateGradeCommand, Result<GradeResponse>>
{
    private readonly IApplicationDbContext _context;

    public UpdateGradeCommandHandler(IApplicationDbContext context)
        => _context = context;

    public async Task<Result<GradeResponse>> Handle(UpdateGradeCommand request, CancellationToken ct)
    {
        var grade = await _context.Grades
            .Include(g => g.Student)
            .FirstOrDefaultAsync(g => g.Id == request.GradeId, ct);
        if (grade is null)
            throw new NotFoundException(nameof(Grade), request.GradeId);
        if (grade.TeacherId != request.TeacherId)
            throw new ForbiddenAccessException();

        grade.Update(request.Value, request.Description);
        await _context.SaveChangesAsync(ct);

        return Result<GradeResponse>.Success(new GradeResponse(
            grade.Id, grade.ClassroomId, grade.StudentId,
            grade.Student.FullName, grade.TeacherId,
            grade.Value, grade.Description, grade.GradedAt
        ));
    }
}
