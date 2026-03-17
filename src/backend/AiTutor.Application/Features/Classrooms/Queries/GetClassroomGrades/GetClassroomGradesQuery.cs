using AiTutor.Application.Common.Models;
using AiTutor.Application.Features.Classrooms.Commands.AddGrade;
using MediatR;

namespace AiTutor.Application.Features.Classrooms.Queries.GetClassroomGrades;

public record GetClassroomGradesQuery(Guid ClassroomId, Guid UserId, bool IsTeacher)
    : IRequest<Result<List<GradeResponse>>>;
