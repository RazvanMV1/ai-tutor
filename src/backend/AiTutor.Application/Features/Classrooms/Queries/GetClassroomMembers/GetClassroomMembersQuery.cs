using AiTutor.Application.Common.Models;
using MediatR;

namespace AiTutor.Application.Features.Classrooms.Queries.GetClassroomMembers;

public record GetClassroomMembersQuery(Guid ClassroomId, Guid TeacherId)
    : IRequest<Result<List<ClassroomMemberDto>>>;

public record ClassroomMemberDto(
    Guid StudentId,
    string FullName,
    string Email,
    DateTime JoinedAt
);
