using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Classrooms.Commands.CreateClassroomLesson;

public class CreateClassroomLessonCommandHandler
    : IRequestHandler<CreateClassroomLessonCommand, Result<ClassroomLessonResponse>>
{
    private readonly IApplicationDbContext _context;

    public CreateClassroomLessonCommandHandler(IApplicationDbContext context)
        => _context = context;

    public async Task<Result<ClassroomLessonResponse>> Handle(
        CreateClassroomLessonCommand request, CancellationToken ct)
    {
        var classroom = await _context.Classrooms
            .FirstOrDefaultAsync(c => c.Id == request.ClassroomId, ct);
        if (classroom is null)
            throw new NotFoundException(nameof(Classroom), request.ClassroomId);
        if (classroom.TeacherId != request.TeacherId)
            throw new ForbiddenAccessException();

        var lesson = ClassroomLesson.Create(
            request.ClassroomId,
            request.Title,
            request.Content,
            request.OrderIndex,
            request.Difficulty
        );

        _context.ClassroomLessons.Add(lesson);
        await _context.SaveChangesAsync(ct);

        return Result<ClassroomLessonResponse>.Success(new ClassroomLessonResponse(
            lesson.Id, lesson.ClassroomId, lesson.Title, lesson.Content,
            lesson.OrderIndex, lesson.Difficulty, lesson.CreatedAt
        ), 201);
    }
}
