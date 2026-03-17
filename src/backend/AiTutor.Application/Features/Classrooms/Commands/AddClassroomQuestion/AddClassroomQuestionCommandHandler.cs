using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Classrooms.Commands.AddClassroomQuestion;

public class AddClassroomQuestionCommandHandler : IRequestHandler<AddClassroomQuestionCommand, Result<ClassroomQuestionResponse>>
{
    private readonly IApplicationDbContext _context;
    public AddClassroomQuestionCommandHandler(IApplicationDbContext context) => _context = context;
    public async Task<Result<ClassroomQuestionResponse>> Handle(AddClassroomQuestionCommand request, CancellationToken ct)
    {
        var quiz = await _context.ClassroomQuizzes
            .FirstOrDefaultAsync(q => q.Id == request.QuizId && q.ClassroomId == request.ClassroomId, ct)
            ?? throw new NotFoundException(nameof(ClassroomQuiz), request.QuizId);
        var classroom = await _context.Classrooms
            .FirstOrDefaultAsync(c => c.Id == request.ClassroomId, ct)
            ?? throw new NotFoundException(nameof(Classroom), request.ClassroomId);
        if (classroom.TeacherId != request.TeacherId)
            throw new ForbiddenAccessException();
        var question = ClassroomQuestion.Create(request.QuizId, request.Text, request.CorrectAnswer, request.Options, request.Points, request.Explanation);
        _context.ClassroomQuestions.Add(question);
        await _context.SaveChangesAsync(ct);
        return Result<ClassroomQuestionResponse>.Success(new ClassroomQuestionResponse(
            question.Id, question.ClassroomQuizId, question.Text, question.Options, question.Points, question.CreatedAt), 201);
    }
}
