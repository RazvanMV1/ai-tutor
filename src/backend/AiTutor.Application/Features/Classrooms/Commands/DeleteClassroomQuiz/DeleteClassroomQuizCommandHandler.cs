using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Classrooms.Commands.DeleteClassroomQuiz;

public class DeleteClassroomQuizCommandHandler
    : IRequestHandler<DeleteClassroomQuizCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;

    public DeleteClassroomQuizCommandHandler(IApplicationDbContext context)
        => _context = context;

    public async Task<Result<bool>> Handle(
        DeleteClassroomQuizCommand request, CancellationToken ct)
    {
        var classroom = await _context.Classrooms
            .FirstOrDefaultAsync(c => c.Id == request.ClassroomId, ct);
        if (classroom is null)
            throw new NotFoundException(nameof(Classroom), request.ClassroomId);
        if (classroom.TeacherId != request.TeacherId)
            throw new ForbiddenAccessException();

        var quiz = await _context.ClassroomQuizzes
            .FirstOrDefaultAsync(q => q.Id == request.QuizId &&
                                      q.ClassroomId == request.ClassroomId, ct);
        if (quiz is null)
            throw new NotFoundException(nameof(ClassroomQuiz), request.QuizId);

        _context.ClassroomQuizzes.Remove(quiz);
        await _context.SaveChangesAsync(ct);

        return Result<bool>.Success(true);
    }
}
