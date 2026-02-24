using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Lessons.Queries.GetLessonById;

public class GetLessonByIdQueryHandler : IRequestHandler<GetLessonByIdQuery, Result<LessonDto>>
{
    private readonly IApplicationDbContext _context;

    public GetLessonByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<LessonDto>> Handle(GetLessonByIdQuery request,
        CancellationToken cancellationToken)
    {
        var lesson = await _context.Lessons
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == request.LessonId, cancellationToken);

        if (lesson is null)
            throw new NotFoundException(nameof(lesson), request.LessonId);

        return Result<LessonDto>.Success(new LessonDto(
            lesson.Id,
            lesson.Title,
            lesson.Content,
            lesson.OrderIndex,
            lesson.Difficulty,
            lesson.SubjectId,
            lesson.CreatedAt));
    }
}
