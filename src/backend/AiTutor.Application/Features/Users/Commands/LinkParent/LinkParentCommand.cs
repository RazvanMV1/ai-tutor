using AiTutor.Application.Common.Models;
using MediatR;

namespace AiTutor.Application.Features.Users.Commands.LinkParent;

public record LinkParentCommand(Guid StudentId, string InvitationCode)
    : IRequest<Result<LinkParentResponse>>;

public record LinkParentResponse(Guid ParentId, string ParentFullName);
