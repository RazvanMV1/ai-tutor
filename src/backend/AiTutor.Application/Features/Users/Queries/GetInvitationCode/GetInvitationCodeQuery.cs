using AiTutor.Application.Common.Models;
using MediatR;

namespace AiTutor.Application.Features.Users.Queries.GetInvitationCode;

public record GetInvitationCodeQuery(Guid ParentId) : IRequest<Result<InvitationCodeDto>>;

public record InvitationCodeDto(Guid ParentId, string InvitationCode);
