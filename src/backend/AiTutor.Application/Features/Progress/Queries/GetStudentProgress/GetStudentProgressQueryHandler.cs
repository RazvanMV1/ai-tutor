using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Progress.Queries.GetStudentProgress;

public class GetStudentProgressQueryHandler
    : IRequestHandler<GetStudentProgressQuery, Result<List<StudentProgressDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetStudentProgressQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<StudentProgressDto>>> Handle(GetStudentProgressQuery request,
        CancellationToken cancellationToken)
    {
        var progresses = await _context.StudentProgresses
            .AsNoTracking()
            .Include(p => p.Lesson)
            .Where(p => p.UserId == request.UserId)
            .Select(p => new StudentProgressDto(
                p.LessonId,
                p.Lesson.Title,
                p.IsCompleted,
                p.ScorePercentage,
                p.AttemptsCount,
                p.CompletedAt))
            .ToListAsync(cancellationToken);

        return Result<List<StudentProgressDto>>.Success(progresses);
    }
}
