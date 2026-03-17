using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Classrooms.Commands.DeleteGrade;

public class DeleteGradeCommandHandler : IRequestHandler<DeleteGradeCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;

    public DeleteGradeCommandHandler(IApplicationDbContext context)
        => _context = context;

    public async Task<Result<bool>> Handle(DeleteGradeCommand request, CancellationToken ct)
    {
        var grade = await _context.Grades
            .FirstOrDefaultAsync(g => g.Id == request.GradeId, ct);
        if (grade is null)
            throw new NotFoundException(nameof(Grade), request.GradeId);
        if (grade.TeacherId != request.TeacherId)
            throw new ForbiddenAccessException();

        _context.Grades.Remove(grade);
        await _context.SaveChangesAsync(ct);
        return Result<bool>.Success(true);
    }
}
