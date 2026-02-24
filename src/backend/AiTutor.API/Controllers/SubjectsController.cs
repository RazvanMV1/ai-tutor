using AiTutor.Application.Features.Subjects.Commands.CreateSubject;
using AiTutor.Application.Features.Subjects.Queries.GetAllSubjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AiTutor.API.Controllers;

[Authorize]
public class SubjectsController : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await Mediator.Send(new GetAllSubjectsQuery());
        return Ok(result.Data);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> Create([FromBody] CreateSubjectCommand command)
    {
        var result = await Mediator.Send(command);
        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, new { message = result.Error });

        return StatusCode(201, result.Data);
    }
}
