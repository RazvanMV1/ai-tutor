using AiTutor.Application.Features.Users.Commands.LinkParent;
using AiTutor.Application.Features.Users.Queries.GetAllUsers;
using AiTutor.Application.Features.Users.Queries.GetChildren;
using AiTutor.Application.Features.Users.Queries.GetInvitationCode;
using AiTutor.Application.Features.Users.Queries.GetUserById;
using AiTutor.Application.Features.Users.Commands.UnlinkParent;
using AiTutor.Application.Features.Users.Queries.GetMyParent;
using Microsoft.AspNetCore.Authorization;
using AiTutor.Application.Features.Users.Queries.GetStudentGrades;
using Microsoft.AspNetCore.Mvc;

namespace AiTutor.API.Controllers;

[Authorize]
public class UsersController : BaseController
{
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Mediator.Send(new GetAllUsersQuery(pageNumber, pageSize));
        return Ok(result.Data);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await Mediator.Send(new GetUserByIdQuery(id));
        return Ok(result.Data);
    }

    /// <summary>Get the invitation code of a Parent</summary>
    [HttpGet("{parentId:guid}/invitation-code")]
    public async Task<IActionResult> GetInvitationCode(Guid parentId)
    {
        var result = await Mediator.Send(new GetInvitationCodeQuery(parentId));
        return result.IsSuccess
            ? Ok(result.Data)
            : StatusCode(result.StatusCode, new { message = result.Error });
    }

    /// <summary>Get all children linked to a Parent</summary>
    [HttpGet("parent/{parentId:guid}/children")]
    public async Task<IActionResult> GetChildren(Guid parentId)
    {
        var result = await Mediator.Send(new GetChildrenQuery(parentId));
        return Ok(result.Data);
    }

    /// <summary>Link a Student to a Parent via invitation code</summary>
    [HttpPost("link-parent")]
    public async Task<IActionResult> LinkParent([FromBody] LinkParentRequest request)
    {
        var result = await Mediator.Send(new LinkParentCommand(request.StudentId, request.InvitationCode));
        return result.IsSuccess
            ? Ok(result.Data)
            : StatusCode(result.StatusCode, new { message = result.Error });
    }

    /// <summary>Get the parent of a Student (or null if not linked)</summary>
    [HttpGet("{studentId:guid}/my-parent")]
    public async Task<IActionResult> GetMyParent(Guid studentId)
    {
        var result = await Mediator.Send(new GetMyParentQuery(studentId));
        return result.IsSuccess
            ? Ok(result.Data)
            : StatusCode(result.StatusCode, new { message = result.Error });
    }

    /// <summary>Unlink a Student from their Parent</summary>
    [HttpDelete("{studentId:guid}/my-parent")]
    public async Task<IActionResult> UnlinkParent(Guid studentId)
    {
        var result = await Mediator.Send(new UnlinkParentCommand(studentId));
        return result.IsSuccess
            ? NoContent()
            : StatusCode(result.StatusCode, new { message = result.Error });
    }

    /// <summary>Get all grades for a Student (only the student or their parent can access)</summary>
    [HttpGet("student/{studentId:guid}/grades")]
    public async Task<IActionResult> GetStudentGrades(Guid studentId, [FromQuery] Guid requesterId)
    {
        var result = await Mediator.Send(new GetStudentGradesQuery(studentId, requesterId));
        return result.IsSuccess
            ? Ok(result.Data)
            : StatusCode(result.StatusCode, new { message = result.Error });
    }



    public record LinkParentRequest(Guid StudentId, string InvitationCode);
}
