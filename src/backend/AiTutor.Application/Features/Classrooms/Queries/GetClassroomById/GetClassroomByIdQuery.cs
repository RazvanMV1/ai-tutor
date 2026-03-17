using AiTutor.Application.Common.Models;
using AiTutor.Application.Features.Classrooms.Commands.CreateClassroom;
using MediatR;

namespace AiTutor.Application.Features.Classrooms.Queries.GetClassroomById;

public record GetClassroomByIdQuery(Guid ClassroomId, Guid UserId) : IRequest<Result<ClassroomResponse>>;
