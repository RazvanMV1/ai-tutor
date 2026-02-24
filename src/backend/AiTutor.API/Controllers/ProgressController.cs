using AiTutor.Application.Features.Progress.Commands.CompleteLesson;
using AiTutor.Application.Features.Progress.Queries.GetStudentProgress;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AiTutor.API.Controllers;

[Authorize]
public class ProgressController : BaseController
{
    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetStudentProgress(Guid userId)
    {
        var result = await Mediator.Send(new GetStudentProgressQuery(userId));
        return Ok(result.Data);
    }

    [HttpPost("complete")]
    public async Task<IActionResult> CompleteLesson([FromBody] CompleteLessonCommand command)
    {
        var result = await Mediator.Send(command);
        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, new { message = result.Error });

        return Ok(result.Data);
    }
}
