using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Application.Features.Classrooms.Commands.CreateClassroom;
using AiTutor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Classrooms.Queries.GetClassroomById;

public class GetClassroomByIdQueryHandler
    : IRequestHandler<GetClassroomByIdQuery, Result<ClassroomResponse>>
{
    private readonly IApplicationDbContext _context;

    public GetClassroomByIdQueryHandler(IApplicationDbContext context)
        => _context = context;

    public async Task<Result<ClassroomResponse>> Handle(
        GetClassroomByIdQuery request, CancellationToken ct)
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
