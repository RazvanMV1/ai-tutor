using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Quizzes.Queries.GetQuizById;

public class GetQuizByIdQueryHandler : IRequestHandler<GetQuizByIdQuery, Result<QuizDetailDto>>
{
    private readonly IApplicationDbContext _context;

    public GetQuizByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<QuizDetailDto>> Handle(GetQuizByIdQuery request,
        CancellationToken cancellationToken)
    {
        var quiz = await _context.Quizzes
            .AsNoTracking()
            .Include(q => q.Questions)
            .FirstOrDefaultAsync(q => q.Id == request.QuizId, cancellationToken);

        if (quiz is null)
            throw new NotFoundException(nameof(quiz), request.QuizId);

        var questionDtos = quiz.Questions.Select(q => new QuestionDto(
            q.Id,
            q.Text,
            q.Options,
            q.Points)).ToList();

        return Result<QuizDetailDto>.Success(new QuizDetailDto(
            quiz.Id,
            quiz.Title,
            quiz.Difficulty,
            quiz.LessonId,
            quiz.TimeLimitMinutes,
            questionDtos));
    }
}
