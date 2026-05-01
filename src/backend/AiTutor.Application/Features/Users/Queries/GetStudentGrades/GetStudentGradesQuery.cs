using AiTutor.Application.Common.Models;
using MediatR;

namespace AiTutor.Application.Features.Users.Queries.GetStudentGrades;

public record GetStudentGradesQuery(Guid StudentId, Guid RequesterId)
    : IRequest<Result<List<StudentGradeDto>>>;

public record StudentGradeDto(
    Guid Id,
    int Value,
    string Description,
    DateTime GradedAt,
    Guid ClassroomId,
    string ClassroomName,
    string SubjectName,
    Guid TeacherId,
    string TeacherName);
