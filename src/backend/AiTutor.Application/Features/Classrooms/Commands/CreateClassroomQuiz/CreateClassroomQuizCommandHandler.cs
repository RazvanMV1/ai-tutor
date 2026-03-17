using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Classrooms.Commands.CreateClassroomQuiz;

public class CreateClassroomQuizCommandHandler : IRequestHandler<CreateClassroomQuizCommand, Result<ClassroomQuizResponse>>
{
    private readonly IApplicationDbContext _context;
    public CreateClassroomQuizCommandHandler(IApplicationDbContext context) => _context = context;
    public async Task<Result<ClassroomQuizResponse>> Handle(CreateClassroomQuizCommand request, CancellationToken ct)
    {
        var lesson = await _context.ClassroomLessons
            .FirstOrDefaultAsync(l => l.Id == request.LessonId && l.ClassroomId == request.ClassroomId, ct)
            ?? throw new NotFoundException(nameof(ClassroomLesson), request.LessonId);
        var classroom = await _context.Classrooms
            .FirstOrDefaultAsync(c => c.Id == request.ClassroomId, ct)
            ?? throw new NotFoundException(nameof(Classroom), request.ClassroomId);
        if (classroom.TeacherId != request.TeacherId)
            throw new ForbiddenAccessException();
        var quiz = ClassroomQuiz.Create(request.ClassroomId, request.LessonId, request.Title, request.Difficulty, request.TimeLimitMinutes);
        _context.ClassroomQuizzes.Add(quiz);
        await _context.SaveChangesAsync(ct);
        return Result<ClassroomQuizResponse>.Success(new ClassroomQuizResponse(
            quiz.Id, quiz.ClassroomLessonId, quiz.ClassroomId, quiz.Title,
            quiz.Difficulty, quiz.TimeLimitMinutes, 0, quiz.CreatedAt), 201);
    }
}
