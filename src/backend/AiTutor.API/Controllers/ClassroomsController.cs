using AiTutor.Application.Features.Classrooms.Commands.AddClassroomQuestion;
using AiTutor.Application.Features.Classrooms.Commands.AddGrade;
using AiTutor.Application.Features.Classrooms.Commands.AddMember;
using AiTutor.Application.Features.Classrooms.Commands.CreateClassroom;
using AiTutor.Application.Features.Classrooms.Commands.CreateClassroomLesson;
using AiTutor.Application.Features.Classrooms.Commands.CreateClassroomQuiz;
using AiTutor.Application.Features.Classrooms.Commands.DeleteClassroom;
using AiTutor.Application.Features.Classrooms.Commands.DeleteGrade;
using AiTutor.Application.Features.Classrooms.Commands.JoinClassroom;
using AiTutor.Application.Features.Classrooms.Commands.RemoveMember;
using AiTutor.Application.Features.Classrooms.Commands.SubmitClassroomQuiz;
using AiTutor.Application.Features.Classrooms.Commands.UpdateClassroom;
using AiTutor.Application.Features.Classrooms.Commands.UpdateGrade;
using AiTutor.Application.Features.Classrooms.Queries.GetClassroomById;
using AiTutor.Application.Features.Classrooms.Queries.GetClassroomGrades;
using AiTutor.Application.Features.Classrooms.Queries.GetClassroomLessons;
using AiTutor.Application.Features.Classrooms.Queries.GetClassroomMembers;
using AiTutor.Application.Features.Classrooms.Queries.GetClassroomProgress;
using AiTutor.Application.Features.Classrooms.Queries.GetMyClassrooms;
using AiTutor.Domain.Enums;
using AiTutor.Application.Features.Classrooms.Commands.UpdateClassroomQuiz;
using AiTutor.Application.Features.Classrooms.Commands.DeleteClassroomQuiz;
using AiTutor.Application.Features.Classrooms.Commands.DeleteClassroomQuestion;
using AiTutor.Application.Features.Classrooms.Queries.GetClassroomQuizzesByLesson;
using AiTutor.Application.Features.Classrooms.Queries.GetClassroomQuizById;
using AiTutor.Application.Features.Classrooms.Commands.UpdateClassroomLesson;
using AiTutor.Application.Features.Classrooms.Commands.DeleteClassroomLesson;
using AiTutor.Application.Features.Classrooms.Queries.GetClassroomLessonById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AiTutor.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClassroomsController : ControllerBase
{
    private readonly IMediator _mediator;
    public ClassroomsController(IMediator mediator) => _mediator = mediator;

    // ── Request DTOs ──────────────────────────────────────────────────────────
    public record CreateClassroomRequest(string Name, string Description, SubjectType SubjectType);
    public record UpdateClassroomRequest(string Name, string? Description);
    public record JoinClassroomRequest(string ClassCode, Guid StudentId);
    public record AddMemberRequest(string StudentEmail);
    public record CreateLessonRequest(string Title, string Content, int OrderIndex, DifficultyLevel Difficulty);
    public record CreateQuizRequest(string Title, DifficultyLevel Difficulty, int TimeLimitMinutes);
    public record AddQuestionRequest(string Text, string CorrectAnswer, List<string> Options, int Points, string? Explanation);
    public record SubmitQuizRequest(List<QuizAnswerDto> Answers);
    public record AddGradeRequest(Guid StudentId, int Value, string? Description);
    public record UpdateGradeRequest(int Value, string? Description);

    // ── Classroom CRUD ────────────────────────────────────────────────────────
    [HttpGet("my")]
    public async Task<IActionResult> GetMyClassrooms([FromQuery] Guid userId)
    {
        var result = await _mediator.Send(new GetMyClassroomsQuery(userId));
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Error);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, [FromQuery] Guid userId)
    {
        var result = await _mediator.Send(new GetClassroomByIdQuery(id, userId));
        return result.IsSuccess ? Ok(result.Data) : NotFound(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateClassroomRequest request, [FromQuery] Guid teacherId)
    {
        var result = await _mediator.Send(new CreateClassroomCommand(request.Name, request.Description, request.SubjectType, teacherId));
        return result.IsSuccess ? StatusCode(result.StatusCode, result.Data) : BadRequest(result.Error);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateClassroomRequest request, [FromQuery] Guid teacherId)
    {
        var result = await _mediator.Send(new UpdateClassroomCommand(id, teacherId, request.Name, request.Description));
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Error);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, [FromQuery] Guid teacherId)
    {
        var result = await _mediator.Send(new DeleteClassroomCommand(id, teacherId));
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    // ── Members ───────────────────────────────────────────────────────────────
    [HttpPost("join")]
    public async Task<IActionResult> Join([FromBody] JoinClassroomRequest request)
    {
        var result = await _mediator.Send(new JoinClassroomCommand(request.ClassCode, request.StudentId));
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    [HttpGet("{id}/members")]
    public async Task<IActionResult> GetMembers(Guid id, [FromQuery] Guid teacherId)
    {
        var result = await _mediator.Send(new GetClassroomMembersQuery(id, teacherId));
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Error);
    }

    [HttpPost("{id}/members")]
    public async Task<IActionResult> AddMember(Guid id, [FromBody] AddMemberRequest request, [FromQuery] Guid teacherId)
    {
        var result = await _mediator.Send(new AddMemberCommand(id, teacherId, request.StudentEmail));
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    [HttpDelete("{id}/members/{studentId}")]
    public async Task<IActionResult> RemoveMember(Guid id, Guid studentId, [FromQuery] Guid teacherId)
    {
        var result = await _mediator.Send(new RemoveMemberCommand(id, teacherId, studentId));
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    // ── Lessons ───────────────────────────────────────────────────────────────
    [HttpGet("{id}/lessons")]
    public async Task<IActionResult> GetLessons(Guid id, [FromQuery] Guid userId)
    {
        var result = await _mediator.Send(new GetClassroomLessonsQuery(id, userId));
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Error);
    }

    [HttpPost("{id}/lessons")]
    public async Task<IActionResult> CreateLesson(Guid id, [FromBody] CreateLessonRequest request, [FromQuery] Guid teacherId)
    {
        var result = await _mediator.Send(new CreateClassroomLessonCommand(id, teacherId, request.Title, request.Content, request.OrderIndex, request.Difficulty));
        return result.IsSuccess ? StatusCode(result.StatusCode, result.Data) : BadRequest(result.Error);
    }

    [HttpGet("{id}/lessons/{lessonId}")]
    public async Task<IActionResult> GetLessonById(Guid id, Guid lessonId, [FromQuery] Guid userId)
    {
        var result = await _mediator.Send(new GetClassroomLessonByIdQuery(id, lessonId, userId));
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Error);
    }

    [HttpPut("{id}/lessons/{lessonId}")]
    public async Task<IActionResult> UpdateLesson(Guid id, Guid lessonId,
        [FromBody] UpdateClassroomLessonRequest request, [FromQuery] Guid teacherId)
    {
        var cmd = new UpdateClassroomLessonCommand(id, lessonId, teacherId,
            request.Title, request.Content, request.Difficulty);
        var result = await _mediator.Send(cmd);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Error);
    }

    [HttpDelete("{id}/lessons/{lessonId}")]
    public async Task<IActionResult> DeleteLesson(Guid id, Guid lessonId, [FromQuery] Guid teacherId)
    {
        var result = await _mediator.Send(new DeleteClassroomLessonCommand(id, lessonId, teacherId));
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Error);
    }


    // ── Quizzes ───────────────────────────────────────────────────────────────
    [HttpPost("{id}/lessons/{lessonId}/quizzes")]
    public async Task<IActionResult> CreateQuiz(Guid id, Guid lessonId, [FromBody] CreateQuizRequest request, [FromQuery] Guid teacherId)
    {
        var result = await _mediator.Send(new CreateClassroomQuizCommand(id, lessonId, teacherId, request.Title, request.Difficulty, request.TimeLimitMinutes));
        return result.IsSuccess ? StatusCode(result.StatusCode, result.Data) : BadRequest(result.Error);
    }

    [HttpPost("{id}/lessons/{lessonId}/quizzes/{quizId}/questions")]
    public async Task<IActionResult> AddQuestion(Guid id, Guid quizId, [FromBody] AddQuestionRequest request, [FromQuery] Guid teacherId)
    {
        var result = await _mediator.Send(new AddClassroomQuestionCommand(id, quizId, teacherId, request.Text, request.CorrectAnswer, request.Options, request.Points, request.Explanation));
        return result.IsSuccess ? StatusCode(result.StatusCode, result.Data) : BadRequest(result.Error);
    }

    [HttpGet("{id}/lessons/{lessonId}/quizzes")]
    public async Task<IActionResult> GetQuizzesByLesson(Guid id, Guid lessonId, [FromQuery] Guid userId)
    {
        var result = await _mediator.Send(new GetClassroomQuizzesByLessonQuery(id, lessonId, userId));
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Error);
    }

    [HttpGet("{id}/quizzes/{quizId}")]
    public async Task<IActionResult> GetQuizById(Guid id, Guid quizId, [FromQuery] Guid userId)
    {
        var result = await _mediator.Send(new GetClassroomQuizByIdQuery(id, quizId, userId));
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Error);
    }

    [HttpPut("{id}/quizzes/{quizId}")]
    public async Task<IActionResult> UpdateQuiz(Guid id, Guid quizId,
        [FromBody] UpdateClassroomQuizRequest request, [FromQuery] Guid teacherId)
    {
        var cmd = new UpdateClassroomQuizCommand(id, quizId, teacherId,
            request.Title, request.Difficulty, request.TimeLimitMinutes);
        var result = await _mediator.Send(cmd);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Error);
    }

    [HttpDelete("{id}/quizzes/{quizId}")]
    public async Task<IActionResult> DeleteQuiz(Guid id, Guid quizId, [FromQuery] Guid teacherId)
    {
        var result = await _mediator.Send(new DeleteClassroomQuizCommand(id, quizId, teacherId));
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Error);
    }

    [HttpDelete("{id}/quizzes/{quizId}/questions/{questionId}")]
    public async Task<IActionResult> DeleteQuestion(Guid id, Guid quizId, Guid questionId, [FromQuery] Guid teacherId)
    {
        var result = await _mediator.Send(new DeleteClassroomQuestionCommand(id, quizId, questionId, teacherId));
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Error);
    }


    [HttpPost("{id}/lessons/{lessonId}/quizzes/{quizId}/submit")]
    public async Task<IActionResult> SubmitQuiz(Guid id, Guid quizId, [FromBody] SubmitQuizRequest request, [FromQuery] Guid studentId)
    {
        var result = await _mediator.Send(new SubmitClassroomQuizCommand(id, quizId, studentId, request.Answers));
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Error);
    }

    // ── Progress ──────────────────────────────────────────────────────────────
    [HttpGet("{id}/progress")]
    public async Task<IActionResult> GetProgress(Guid id, [FromQuery] Guid userId)
    {
        var result = await _mediator.Send(new GetClassroomProgressQuery(id, userId));
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Error);
    }

    // ── Grades / Catalog ──────────────────────────────────────────────────────
    [HttpGet("{id}/grades")]
    public async Task<IActionResult> GetGrades(Guid id, [FromQuery] Guid userId, [FromQuery] bool isTeacher = false)
    {
        var result = await _mediator.Send(new GetClassroomGradesQuery(id, userId, isTeacher));
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Error);
    }

    [HttpPost("{id}/grades")]
    public async Task<IActionResult> AddGrade(Guid id, [FromBody] AddGradeRequest request, [FromQuery] Guid teacherId)
    {
        var result = await _mediator.Send(new AddGradeCommand(id, request.StudentId, teacherId, request.Value, request.Description ?? string.Empty));
        return result.IsSuccess ? StatusCode(result.StatusCode, result.Data) : BadRequest(result.Error);
    }

    [HttpPut("{id}/grades/{gradeId}")]
    public async Task<IActionResult> UpdateGrade(Guid id, Guid gradeId, [FromBody] UpdateGradeRequest request, [FromQuery] Guid teacherId)
    {
        var result = await _mediator.Send(new UpdateGradeCommand(gradeId, teacherId, request.Value, request.Description ?? string.Empty));
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Error);
    }

    [HttpDelete("{id}/grades/{gradeId}")]
    public async Task<IActionResult> DeleteGrade(Guid id, Guid gradeId, [FromQuery] Guid teacherId)
    {
        var result = await _mediator.Send(new DeleteGradeCommand(gradeId, teacherId));
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    public record UpdateClassroomLessonRequest(
    string Title,
    string Content,
    AiTutor.Domain.Enums.DifficultyLevel Difficulty
);

public record UpdateClassroomQuizRequest(
    string Title,
    AiTutor.Domain.Enums.DifficultyLevel Difficulty,
    int TimeLimitMinutes
);


}



