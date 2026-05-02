using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Classrooms.Queries.GetClassroomQuizById;

public class GetClassroomQuizByIdQueryHandler
    : IRequestHandler<GetClassroomQuizByIdQuery, Result<ClassroomQuizDetailResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetClassroomQuizByIdQueryHandler(IApplicationDbContext context)
        => _context = context;

    public async Task<Result<ClassroomQuizDetailResponse>> Handle(
        GetClassroomQuizByIdQuery request, CancellationToken ct)
    {
        var classroom = await _context.Classrooms
            .FirstOrDefaultAsync(c => c.Id == request.ClassroomId, ct);
        if (classroom is null)
            throw new NotFoundException(nameof(Classroom), request.ClassroomId);

        var isTeacher = classroom.TeacherId == request.UserId;
        var isStudent = await _context.ClassroomMembers
            .AnyAsync(m => m.ClassroomId == request.ClassroomId &&
                          m.StudentId == request.UserId && m.IsActive, ct);
        if (!isTeacher && !isStudent)
            throw new ForbiddenAccessException();

        var quiz = await _context.ClassroomQuizzes
            .Include(q => q.Questions)
            .FirstOrDefaultAsync(q => q.Id == request.QuizId &&
                                      q.ClassroomId == request.ClassroomId, ct);
        if (quiz is null)
            throw new NotFoundException(nameof(ClassroomQuiz), request.QuizId);

        var questions = quiz.Questions.Select(q => new ClassroomQuestionDetailResponse(
            q.Id, q.Text, q.CorrectAnswer, q.Options, q.Points, q.Explanation
        )).ToList();

        return Result<ClassroomQuizDetailResponse>.Success(new ClassroomQuizDetailResponse(
            quiz.Id, quiz.ClassroomLessonId, quiz.ClassroomId, quiz.Title,
            quiz.Difficulty, quiz.TimeLimitMinutes, quiz.CreatedAt, questions
        ));
    }
}
