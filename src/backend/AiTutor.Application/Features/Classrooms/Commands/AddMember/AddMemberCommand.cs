using AiTutor.Application.Common.Models;
using MediatR;

namespace AiTutor.Application.Features.Classrooms.Commands.AddMember;

public record AddMemberCommand(Guid ClassroomId, Guid TeacherId, string StudentEmail)
    : IRequest<Result<bool>>;
