using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Classrooms.Commands.DeleteClassroom;

public class DeleteClassroomCommandHandler : IRequestHandler<DeleteClassroomCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;

    public DeleteClassroomCommandHandler(IApplicationDbContext context)
        => _context = context;

    public async Task<Result<bool>> Handle(DeleteClassroomCommand request, CancellationToken ct)
    {
        var classroom = await _context.Classrooms
            .FirstOrDefaultAsync(c => c.Id == request.ClassroomId, ct);
        if (classroom is null)
            throw new NotFoundException(nameof(Classroom), request.ClassroomId);
        if (classroom.TeacherId != request.TeacherId)
            throw new ForbiddenAccessException();

        classroom.Deactivate();
        await _context.SaveChangesAsync(ct);
        return Result<bool>.Success(true);
    }
}
