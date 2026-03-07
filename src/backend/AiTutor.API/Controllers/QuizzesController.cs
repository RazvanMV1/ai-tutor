using AiTutor.Application.Features.Quizzes.Commands.CreateQuiz;
using AiTutor.Application.Features.Quizzes.Commands.SubmitQuiz;
using AiTutor.Application.Features.Quizzes.Queries.GetQuizById;
using AiTutor.Application.Features.Quizzes.Queries.GetQuizzesByLesson;
using AiTutor.Application.Features.Questions.Commands.CreateQuestion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AiTutor.API.Controllers;

[Authorize]
public class QuizzesController : BaseController
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await Mediator.Send(new GetQuizByIdQuery(id));
        return Ok(result.Data);
    }

    [HttpGet("lesson/{lessonId:guid}")]
    public async Task<IActionResult> GetByLesson(Guid lessonId)
    {
        var result = await Mediator.Send(new GetQuizzesByLessonQuery(lessonId));
        return Ok(result.Data);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> Create([FromBody] CreateQuizCommand command)
    {
        var result = await Mediator.Send(command);
        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, new { message = result.Error });
        return StatusCode(201, result.Data);
    }

    [HttpPost("{id:guid}/questions")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> AddQuestion(Guid id,
        [FromBody] CreateQuestionCommand command)
    {
        var commandWithId = command with { QuizId = id };
        var result = await Mediator.Send(commandWithId);
        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, new { message = result.Error });
        return StatusCode(201, result.Data);
    }

    [HttpPost("{id:guid}/submit")]
    public async Task<IActionResult> Submit(Guid id,
        [FromBody] SubmitQuizCommand command)
    {
        var commandWithId = command with { QuizId = id };
        var result = await Mediator.Send(commandWithId);
        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, new { message = result.Error });
        return Ok(result.Data);
    }
}
