using AiTutor.Application.Features.AI.Queries.GenerateProblems;
using AiTutor.Application.Features.AI.Queries.GetExplanation;
using AiTutor.Application.Features.AI.Queries.GetHint;
using AiTutor.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AiTutor.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AiController : ControllerBase
{
    private readonly IMediator _mediator;
    public AiController(IMediator mediator) => _mediator = mediator;

    public record ExplanationRequest(
        string Topic,
        SubjectType Subject,
        DifficultyLevel DifficultyLevel,
        int StudentAge
    );

    public record HintRequest(
        string Question,
        SubjectType Subject
    );

    public record ProblemsRequest(
        string Topic,
        SubjectType Subject,
        DifficultyLevel DifficultyLevel,
        int StudentAge,
        int Count = 3
    );

    /// <summary>Get AI explanation for a topic</summary>
    [HttpPost("explanation")]
    public async Task<IActionResult> GetExplanation([FromBody] ExplanationRequest request)
    {
        var result = await _mediator.Send(new GetExplanationQuery(
            request.Topic,
            request.Subject,
            request.DifficultyLevel,
            request.StudentAge
        ));
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Error);
    }

    /// <summary>Get AI hint for a question</summary>
    [HttpPost("hint")]
    public async Task<IActionResult> GetHint([FromBody] HintRequest request)
    {
        var result = await _mediator.Send(new GetHintQuery(
            request.Question,
            request.Subject
        ));
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Error);
    }

    /// <summary>Generate AI problems for a topic</summary>
    [HttpPost("problems")]
    public async Task<IActionResult> GenerateProblems([FromBody] ProblemsRequest request)
    {
        var result = await _mediator.Send(new GenerateProblemsQuery(
            request.Topic,
            request.Subject,
            request.DifficultyLevel,
            request.StudentAge,
            request.Count
        ));
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Error);
    }
}
