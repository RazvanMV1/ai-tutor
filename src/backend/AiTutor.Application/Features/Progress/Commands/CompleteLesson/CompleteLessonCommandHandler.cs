using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Progress.Commands.CompleteLesson;

public class CompleteLessonCommandHandler : IRequestHandler<CompleteLessonCommand, Result<ProgressResponse>>
{
    private readonly IApplicationDbContext _context;

    public CompleteLessonCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<ProgressResponse>> Handle(CompleteLessonCommand request,
        CancellationToken cancellationToken)
    {
        var progress = await _context.StudentProgresses
            .FirstOrDefaultAsync(p => p.UserId == request.UserId
                && p.LessonId == request.LessonId, cancellationToken);

        if (progress is null)
        {
            progress = StudentProgress.Create(request.UserId, request.LessonId);
            _context.StudentProgresses.Add(progress);
        }

        progress.Complete(request.ScorePercentage);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<ProgressResponse>.Success(
            new ProgressResponse(progress.Id, progress.IsCompleted,
                progress.ScorePercentage, progress.CompletedAt));
    }
}
