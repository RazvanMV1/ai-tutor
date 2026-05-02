using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Application.Features.Classrooms.Commands.CreateClassroomLesson;
using AiTutor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Classrooms.Commands.UpdateClassroomLesson;

public class UpdateClassroomLessonCommandHandler
    : IRequestHandler<UpdateClassroomLessonCommand, Result<ClassroomLessonResponse>>
{
    private readonly IApplicationDbContext _context;

    public UpdateClassroomLessonCommandHandler(IApplicationDbContext context)
        => _context = context;

    public async Task<Result<ClassroomLessonResponse>> Handle(
        UpdateClassroomLessonCommand request, CancellationToken ct)
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

        lesson.Update(request.Title, request.Content, request.Difficulty);
        await _context.SaveChangesAsync(ct);

        return Result<ClassroomLessonResponse>.Success(new ClassroomLessonResponse(
            lesson.Id, lesson.ClassroomId, lesson.Title, lesson.Content,
            lesson.OrderIndex, lesson.Difficulty, lesson.CreatedAt
        ));
    }
}
