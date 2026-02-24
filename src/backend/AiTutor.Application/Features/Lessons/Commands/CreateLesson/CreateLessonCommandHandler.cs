using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Lessons.Commands.CreateLesson;

public class CreateLessonCommandHandler : IRequestHandler<CreateLessonCommand, Result<LessonResponse>>
{
    private readonly IApplicationDbContext _context;

    public CreateLessonCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<LessonResponse>> Handle(CreateLessonCommand request,
        CancellationToken cancellationToken)
    {
        var subjectExists = await _context.Subjects
            .AnyAsync(s => s.Id == request.SubjectId, cancellationToken);

        if (!subjectExists)
            throw new NotFoundException(nameof(Subject), request.SubjectId);

        var lesson = Lesson.Create(
            request.Title,
            request.Content,
            request.OrderIndex,
            request.Difficulty,
            request.SubjectId);

        _context.Lessons.Add(lesson);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<LessonResponse>.Success(
            new LessonResponse(lesson.Id, lesson.Title, lesson.Difficulty, lesson.SubjectId), 201);
    }
}
