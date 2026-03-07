using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Quizzes.Commands.SubmitQuiz;

public class SubmitQuizCommandHandler : IRequestHandler<SubmitQuizCommand, Result<QuizResultResponse>>
{
    private readonly IApplicationDbContext _context;

    public SubmitQuizCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<QuizResultResponse>> Handle(SubmitQuizCommand request,
        CancellationToken cancellationToken)
    {
        var quiz = await _context.Quizzes
            .Include(q => q.Questions)
            .FirstOrDefaultAsync(q => q.Id == request.QuizId, cancellationToken);

        if (quiz is null)
            throw new NotFoundException(nameof(quiz), request.QuizId);

        var results = new List<QuestionResultDto>();
        int correctAnswers = 0;

        foreach (var answer in request.Answers)
        {
            var question = quiz.Questions
                .FirstOrDefault(q => q.Id == answer.QuestionId);

            if (question is null) continue;

            var isCorrect = question.IsCorrect(answer.Answer);
            if (isCorrect) correctAnswers++;

            results.Add(new QuestionResultDto(
                question.Id,
                question.Text,
                answer.Answer,
                question.CorrectAnswer,
                isCorrect,
                question.Explanation));
        }

        var totalQuestions = quiz.Questions.Count;
        var scorePercentage = totalQuestions > 0
            ? (int)Math.Round((double)correctAnswers / totalQuestions * 100)
            : 0;

        return Result<QuizResultResponse>.Success(new QuizResultResponse(
            request.QuizId,
            request.UserId,
            totalQuestions,
            correctAnswers,
            scorePercentage,
            results));
    }
}
