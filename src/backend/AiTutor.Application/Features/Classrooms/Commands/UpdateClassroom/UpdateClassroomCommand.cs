using AiTutor.Application.Common.Models;
using AiTutor.Application.Features.Classrooms.Commands.CreateClassroom;
using MediatR;

namespace AiTutor.Application.Features.Classrooms.Commands.UpdateClassroom;

public record UpdateClassroomCommand(
    Guid ClassroomId,
    Guid TeacherId,
    string Name,
    string? Description
) : IRequest<Result<ClassroomResponse>>;
