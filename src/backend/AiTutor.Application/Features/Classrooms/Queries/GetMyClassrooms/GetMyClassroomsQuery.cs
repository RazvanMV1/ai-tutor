using AiTutor.Application.Common.Models;
using AiTutor.Application.Features.Classrooms.Commands.CreateClassroom;
using MediatR;

namespace AiTutor.Application.Features.Classrooms.Queries.GetMyClassrooms;

public record GetMyClassroomsQuery(Guid UserId) : IRequest<Result<List<ClassroomResponse>>>;
