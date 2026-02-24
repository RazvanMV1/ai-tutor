using AiTutor.Application.Features.Lessons.Commands.CreateLesson;
using AiTutor.Application.Features.Lessons.Queries.GetLessonById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AiTutor.API.Controllers;

[Authorize]
public class LessonsController : BaseController
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await Mediator.Send(new GetLessonByIdQuery(id));
        return Ok(result.Data);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> Create([FromBody] CreateLessonCommand command)
    {
        var result = await Mediator.Send(command);
        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, new { message = result.Error });

        return StatusCode(201, result.Data);
    }
}
