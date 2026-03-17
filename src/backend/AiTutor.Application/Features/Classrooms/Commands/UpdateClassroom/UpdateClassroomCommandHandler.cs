using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Application.Features.Classrooms.Commands.CreateClassroom;
using AiTutor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Classrooms.Commands.UpdateClassroom;

public class UpdateClassroomCommandHandler : IRequestHandler<UpdateClassroomCommand, Result<ClassroomResponse>>
{
    private readonly IApplicationDbContext _context;

    public UpdateClassroomCommandHandler(IApplicationDbContext context)
        => _context = context;

    public async Task<Result<ClassroomResponse>> Handle(UpdateClassroomCommand request, CancellationToken ct)
    {
        var classroom = await _context.Classrooms
            .FirstOrDefaultAsync(c => c.Id == request.ClassroomId, ct);
        if (classroom is null)
            throw new NotFoundException(nameof(Classroom), request.ClassroomId);
        if (classroom.TeacherId != request.TeacherId)
            throw new ForbiddenAccessException();

        classroom.Update(request.Name, request.Description);
        await _context.SaveChangesAsync(ct);

        var memberCount = await _context.ClassroomMembers
            .CountAsync(m => m.ClassroomId == classroom.Id && m.IsActive, ct);
        var lessonCount = await _context.ClassroomLessons
            .CountAsync(l => l.ClassroomId == classroom.Id, ct);

        return Result<ClassroomResponse>.Success(new ClassroomResponse(
            classroom.Id, classroom.Name, classroom.Description,
            classroom.SubjectType, classroom.TeacherId, classroom.ClassCode,
            classroom.MaxStudents, classroom.IsActive, memberCount, lessonCount
        ));
    }
}
