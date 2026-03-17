using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Domain.Entities;
using AiTutor.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Classrooms.Commands.CreateClassroom;

public class CreateClassroomCommandHandler : IRequestHandler<CreateClassroomCommand, Result<ClassroomResponse>>
{
    private readonly IApplicationDbContext _context;

    public CreateClassroomCommandHandler(IApplicationDbContext context)
        => _context = context;

    public async Task<Result<ClassroomResponse>> Handle(CreateClassroomCommand request, CancellationToken ct)
    {
        // Verifică că profesorul există
        var teacher = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.TeacherId, ct);
        if (teacher is null)
            throw new NotFoundException(nameof(User), request.TeacherId);

        // Verifică abonament activ Teacher
        var hasActiveSubscription = await _context.Subscriptions
            .AnyAsync(s => s.UserId == request.TeacherId &&
                          s.IsActive &&
                          s.EndDate >= DateTime.UtcNow, ct);
        if (!hasActiveSubscription)
            return Result<ClassroomResponse>.Failure("Ai nevoie de un abonament activ pentru a crea clase.", 403);

        // Verifică limita de 5 clase
        var classroomCount = await _context.Classrooms
            .CountAsync(c => c.TeacherId == request.TeacherId && c.IsActive, ct);
        if (classroomCount >= 5)
            return Result<ClassroomResponse>.Failure("Ai atins limita maximă de 5 clase active.", 400);

        var classroom = Classroom.Create(
            request.Name,
            request.Description,
            request.SubjectType,
            request.TeacherId
        );

        _context.Classrooms.Add(classroom);
        await _context.SaveChangesAsync(ct);

        return Result<ClassroomResponse>.Success(new ClassroomResponse(
            classroom.Id,
            classroom.Name,
            classroom.Description,
            classroom.SubjectType,
            classroom.TeacherId,
            classroom.ClassCode,
            classroom.MaxStudents,
            classroom.IsActive,
            0, 0
        ), 201);
    }
}
