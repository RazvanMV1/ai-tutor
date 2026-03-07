using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Quizzes.Commands.CreateQuiz;

public class CreateQuizCommandHandler : IRequestHandler<CreateQuizCommand, Result<QuizResponse>>
{
    private readonly IApplicationDbContext _context;

    public CreateQuizCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<QuizResponse>> Handle(CreateQuizCommand request,
        CancellationToken cancellationToken)
    {
        var lessonExists = await _context.Lessons
            .AnyAsync(l => l.Id == request.LessonId, cancellationToken);

        if (!lessonExists)
            throw new NotFoundException(nameof(Lesson), request.LessonId);

        var quiz = Quiz.Create(
            request.Title,
            request.Difficulty,
            request.LessonId,
            request.TimeLimitMinutes);

        _context.Quizzes.Add(quiz);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<QuizResponse>.Success(
            new QuizResponse(
                quiz.Id,
                quiz.Title,
                quiz.Difficulty,
                quiz.LessonId,
                quiz.TimeLimitMinutes), 201);
    }
}
