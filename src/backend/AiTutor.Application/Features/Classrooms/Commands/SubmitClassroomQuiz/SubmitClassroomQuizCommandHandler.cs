using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Classrooms.Commands.SubmitClassroomQuiz;

public class SubmitClassroomQuizCommandHandler : IRequestHandler<SubmitClassroomQuizCommand, Result<ClassroomQuizResultResponse>>
{
    private readonly IApplicationDbContext _context;
    public SubmitClassroomQuizCommandHandler(IApplicationDbContext context) => _context = context;
    public async Task<Result<ClassroomQuizResultResponse>> Handle(SubmitClassroomQuizCommand request, CancellationToken ct)
    {
        var isMember = await _context.ClassroomMembers
            .AnyAsync(m => m.ClassroomId == request.ClassroomId && m.StudentId == request.StudentId && m.IsActive, ct);
        if (!isMember) throw new ForbiddenAccessException();
        var quiz = await _context.ClassroomQuizzes
            .Include(q => q.Questions)
            .FirstOrDefaultAsync(q => q.Id == request.QuizId && q.ClassroomId == request.ClassroomId, ct)
            ?? throw new NotFoundException(nameof(ClassroomQuiz), request.QuizId);
        var results = quiz.Questions.Select(q => {
            var answer = request.Answers.FirstOrDefault(a => a.QuestionId == q.Id);
            var isCorrect = answer != null && q.IsCorrect(answer.Answer);
            return new QuizQuestionResult(q.Id, q.Text, answer?.Answer ?? "", q.CorrectAnswer, isCorrect, q.Explanation);
        }).ToList();
        var correct = results.Count(r => r.IsCorrect);
        var scoreDouble = quiz.Questions.Count > 0 ? Math.Round((double)correct / quiz.Questions.Count * 100, 2) : 0;
        var scoreInt = (int)Math.Round(scoreDouble);
        var progress = await _context.ClassroomProgresses
            .FirstOrDefaultAsync(p => p.ClassroomId == request.ClassroomId && p.StudentId == request.StudentId && p.ClassroomLessonId == quiz.ClassroomLessonId, ct);
        if (progress == null)
        {
            progress = ClassroomProgress.Create(request.ClassroomId, request.StudentId, quiz.ClassroomLessonId);
            _context.ClassroomProgresses.Add(progress);
        }
        progress.RegisterAttempt(scoreInt);
        await _context.SaveChangesAsync(ct);
        return Result<ClassroomQuizResultResponse>.Success(new ClassroomQuizResultResponse(
            quiz.Id, request.StudentId, quiz.Questions.Count, correct, scoreDouble, results));
    }
}

