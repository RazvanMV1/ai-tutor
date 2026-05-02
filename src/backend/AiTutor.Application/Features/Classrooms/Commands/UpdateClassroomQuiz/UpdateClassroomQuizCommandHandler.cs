using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Application.Features.Classrooms.Commands.CreateClassroomQuiz;
using AiTutor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Classrooms.Commands.UpdateClassroomQuiz;

public class UpdateClassroomQuizCommandHandler
    : IRequestHandler<UpdateClassroomQuizCommand, Result<ClassroomQuizResponse>>
{
    private readonly IApplicationDbContext _context;

    public UpdateClassroomQuizCommandHandler(IApplicationDbContext context)
        => _context = context;

    public async Task<Result<ClassroomQuizResponse>> Handle(
        UpdateClassroomQuizCommand request, CancellationToken ct)
    {
        var classroom = await _context.Classrooms
            .FirstOrDefaultAsync(c => c.Id == request.ClassroomId, ct);
        if (classroom is null)
            throw new NotFoundException(nameof(Classroom), request.ClassroomId);
        if (classroom.TeacherId != request.TeacherId)
            throw new ForbiddenAccessException();

        var quiz = await _context.ClassroomQuizzes
            .Include(q => q.Questions)
            .FirstOrDefaultAsync(q => q.Id == request.QuizId &&
                                      q.ClassroomId == request.ClassroomId, ct);
        if (quiz is null)
            throw new NotFoundException(nameof(ClassroomQuiz), request.QuizId);

        quiz.Update(request.Title, request.Difficulty, request.TimeLimitMinutes);
        await _context.SaveChangesAsync(ct);

        return Result<ClassroomQuizResponse>.Success(new ClassroomQuizResponse(
            quiz.Id, quiz.ClassroomLessonId, quiz.ClassroomId, quiz.Title,
            quiz.Difficulty, quiz.TimeLimitMinutes, quiz.Questions.Count, quiz.CreatedAt
        ));
    }
}
