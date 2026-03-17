using AiTutor.Application.Common.Models;
using MediatR;

namespace AiTutor.Application.Features.Classrooms.Commands.JoinClassroom;

public record JoinClassroomCommand(string ClassCode, Guid StudentId) : IRequest<Result<bool>>;
