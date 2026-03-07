using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Questions.Commands.CreateQuestion;

public class CreateQuestionCommandHandler
    : IRequestHandler<CreateQuestionCommand, Result<QuestionResponse>>
{
    private readonly IApplicationDbContext _context;

    public CreateQuestionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<QuestionResponse>> Handle(CreateQuestionCommand request,
        CancellationToken cancellationToken)
    {
        var quizExists = await _context.Quizzes
            .AnyAsync(q => q.Id == request.QuizId, cancellationToken);

        if (!quizExists)
            throw new NotFoundException(nameof(Quiz), request.QuizId);

        var question = Question.Create(
            request.Text,
            request.CorrectAnswer,
            request.Options,
            request.Points,
            request.QuizId,
            request.Explanation);

        _context.Questions.Add(question);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<QuestionResponse>.Success(
            new QuestionResponse(
                question.Id,
                question.Text,
                question.Options,
                question.Points,
                question.QuizId), 201);
    }
}
