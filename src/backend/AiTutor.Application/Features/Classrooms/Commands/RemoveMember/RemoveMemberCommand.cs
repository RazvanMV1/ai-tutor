using AiTutor.Application.Common.Models;
using MediatR;

namespace AiTutor.Application.Features.Classrooms.Commands.RemoveMember;

public record RemoveMemberCommand(Guid ClassroomId, Guid TeacherId, Guid StudentId)
    : IRequest<Result<bool>>;
