using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Classrooms.Commands.DeleteClassroomLesson;

public class DeleteClassroomLessonCommandHandler
    : IRequestHandler<DeleteClassroomLessonCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;

    public DeleteClassroomLessonCommandHandler(IApplicationDbContext context)
        => _context = context;

    public async Task<Result<bool>> Handle(
        DeleteClassroomLessonCommand request, CancellationToken ct)
    {
        var classroom = await _context.Classrooms
            .FirstOrDefaultAsync(c => c.Id == request.ClassroomId, ct);
        if (classroom is null)
            throw new NotFoundException(nameof(Classroom), request.ClassroomId);
        if (classroom.TeacherId != request.TeacherId)
            throw new ForbiddenAccessException();

        var lesson = await _context.ClassroomLessons
            .FirstOrDefaultAsync(l => l.Id == request.LessonId &&
                                      l.ClassroomId == request.ClassroomId, ct);
        if (lesson is null)
            throw new NotFoundException(nameof(ClassroomLesson), request.LessonId);

        _context.ClassroomLessons.Remove(lesson);
        await _context.SaveChangesAsync(ct);

        return Result<bool>.Success(true);
    }
}
