using AiTutor.Application.Features.Subscriptions.Commands.CancelSubscription;
using AiTutor.Application.Features.Subscriptions.Commands.CreateSubscription;
using AiTutor.Application.Features.Subscriptions.Queries.GetAllSubscriptions;
using AiTutor.Application.Features.Subscriptions.Queries.GetSubscriptionByUser;
using AiTutor.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AiTutor.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SubscriptionsController : ControllerBase
{
    private readonly IMediator _mediator;
    public SubscriptionsController(IMediator mediator) => _mediator = mediator;

    public record CreateSubscriptionRequest(
        Guid UserId,
        SubscriptionType Type,
        decimal Price,
        DateTime StartDate,
        DateTime EndDate
    );

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _mediator.Send(new GetAllSubscriptionsQuery(page, pageSize));
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Error);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUser(Guid userId)
    {
        var result = await _mediator.Send(new GetSubscriptionByUserQuery(userId));
        return result.IsSuccess ? Ok(result.Data) : NotFound(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSubscriptionRequest request)
    {
        var command = new CreateSubscriptionCommand(
            request.UserId,
            request.Type,
            request.Price,
            request.StartDate,
            request.EndDate
        );
        var result = await _mediator.Send(command);
        return result.IsSuccess
            ? StatusCode(result.StatusCode, result.Data)
            : BadRequest(result.Error);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Cancel(Guid id, [FromQuery] Guid userId)
    {
        var result = await _mediator.Send(new CancelSubscriptionCommand(id, userId));
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
}
