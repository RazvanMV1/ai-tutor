using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Classrooms.Commands.DeleteClassroomQuestion;

public class DeleteClassroomQuestionCommandHandler
    : IRequestHandler<DeleteClassroomQuestionCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;

    public DeleteClassroomQuestionCommandHandler(IApplicationDbContext context)
        => _context = context;

    public async Task<Result<bool>> Handle(
        DeleteClassroomQuestionCommand request, CancellationToken ct)
    {
        var classroom = await _context.Classrooms
            .FirstOrDefaultAsync(c => c.Id == request.ClassroomId, ct);
        if (classroom is null)
            throw new NotFoundException(nameof(Classroom), request.ClassroomId);
        if (classroom.TeacherId != request.TeacherId)
            throw new ForbiddenAccessException();

        var question = await _context.ClassroomQuestions
            .FirstOrDefaultAsync(q => q.Id == request.QuestionId &&
                                      q.ClassroomQuizId == request.QuizId, ct);
        if (question is null)
            throw new NotFoundException(nameof(ClassroomQuestion), request.QuestionId);

        _context.ClassroomQuestions.Remove(question);
        await _context.SaveChangesAsync(ct);

        return Result<bool>.Success(true);
    }
}
