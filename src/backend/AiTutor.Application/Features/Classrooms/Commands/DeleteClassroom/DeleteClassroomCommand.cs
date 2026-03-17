using AiTutor.Application.Common.Models;
using MediatR;

namespace AiTutor.Application.Features.Classrooms.Commands.DeleteClassroom;

public record DeleteClassroomCommand(Guid ClassroomId, Guid TeacherId) : IRequest<Result<bool>>;
