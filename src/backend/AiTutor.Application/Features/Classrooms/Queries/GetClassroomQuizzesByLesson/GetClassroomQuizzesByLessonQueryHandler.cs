using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Application.Features.Classrooms.Commands.CreateClassroomQuiz;
using AiTutor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Classrooms.Queries.GetClassroomQuizzesByLesson;

public class GetClassroomQuizzesByLessonQueryHandler
    : IRequestHandler<GetClassroomQuizzesByLessonQuery, Result<List<ClassroomQuizResponse>>>
{
    private readonly IApplicationDbContext _context;

    public GetClassroomQuizzesByLessonQueryHandler(IApplicationDbContext context)
        => _context = context;

    public async Task<Result<List<ClassroomQuizResponse>>> Handle(
        GetClassroomQuizzesByLessonQuery request, CancellationToken ct)
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

        var quizzes = await _context.ClassroomQuizzes
            .Include(q => q.Questions)
            .Where(q => q.ClassroomLessonId == request.LessonId &&
                        q.ClassroomId == request.ClassroomId)
            .OrderBy(q => q.CreatedAt)
            .ToListAsync(ct);

        var result = quizzes.Select(q => new ClassroomQuizResponse(
            q.Id, q.ClassroomLessonId, q.ClassroomId, q.Title,
            q.Difficulty, q.TimeLimitMinutes, q.Questions.Count, q.CreatedAt
        )).ToList();

        return Result<List<ClassroomQuizResponse>>.Success(result);
    }
}
