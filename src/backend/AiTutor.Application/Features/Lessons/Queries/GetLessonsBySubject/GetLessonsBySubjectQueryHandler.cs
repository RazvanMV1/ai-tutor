using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Lessons.Queries.GetLessonsBySubject;

public class GetLessonsBySubjectQueryHandler
    : IRequestHandler<GetLessonsBySubjectQuery, Result<List<LessonDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetLessonsBySubjectQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<LessonDto>>> Handle(
        GetLessonsBySubjectQuery request,
        CancellationToken cancellationToken)
    {
        var lessons = await _context.Lessons
            .AsNoTracking()
            .Where(l => l.SubjectId == request.SubjectId)
            .OrderBy(l => l.OrderIndex)
            .Select(l => new LessonDto(
                l.Id,
                l.Title,
                l.Content,
                l.OrderIndex,
                l.Difficulty,
                l.SubjectId,
                l.CreatedAt))
            .ToListAsync(cancellationToken);

        return Result<List<LessonDto>>.Success(lessons);
    }
}
