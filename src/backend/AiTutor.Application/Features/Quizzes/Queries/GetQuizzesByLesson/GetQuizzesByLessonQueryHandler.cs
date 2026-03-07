using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Quizzes.Queries.GetQuizzesByLesson;

public class GetQuizzesByLessonQueryHandler
    : IRequestHandler<GetQuizzesByLessonQuery, Result<List<QuizSummaryDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetQuizzesByLessonQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<QuizSummaryDto>>> Handle(GetQuizzesByLessonQuery request,
        CancellationToken cancellationToken)
    {
        var quizzes = await _context.Quizzes
            .AsNoTracking()
            .Include(q => q.Questions)
            .Where(q => q.LessonId == request.LessonId)
            .Select(q => new QuizSummaryDto(
                q.Id,
                q.Title,
                q.Difficulty,
                q.TimeLimitMinutes,
                q.Questions.Count))
            .ToListAsync(cancellationToken);

        return Result<List<QuizSummaryDto>>.Success(quizzes);
    }
}
