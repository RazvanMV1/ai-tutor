using AiTutor.Application.Common.Models;
using AiTutor.Domain.Enums;
using MediatR;

namespace AiTutor.Application.Features.Classrooms.Commands.CreateClassroom;

public record CreateClassroomCommand(
    string Name,
    string Description,
    SubjectType SubjectType,
    Guid TeacherId
) : IRequest<Result<ClassroomResponse>>;

public record ClassroomResponse(
    Guid Id,
    string Name,
    string Description,
    SubjectType SubjectType,
    Guid TeacherId,
    string ClassCode,
    int MaxStudents,
    bool IsActive,
    int MemberCount,
    int LessonCount
);
