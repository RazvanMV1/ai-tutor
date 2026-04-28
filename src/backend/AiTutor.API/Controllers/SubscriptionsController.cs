using AiTutor.Application.Features.Subscriptions.Commands.CancelSubscription;
using AiTutor.Application.Features.Subscriptions.Commands.CreateCheckoutSession;
using AiTutor.Application.Features.Subscriptions.Commands.CreateSubscription;
using AiTutor.Application.Features.Subscriptions.Commands.HandleStripeWebhook;
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

    public record CreateCheckoutSessionRequest(
        Guid UserId,
        SubscriptionType SubscriptionType
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

    /// <summary>
    /// Pornește un Stripe Checkout pentru utilizatorul curent.
    /// Returnează URL-ul la care frontend-ul redirectează user-ul.
    /// </summary>
    [HttpPost("checkout-session")]
    public async Task<IActionResult> CreateCheckoutSession(
        [FromBody] CreateCheckoutSessionRequest request)
    {
        var command = new CreateCheckoutSessionCommand(
            request.UserId,
            request.SubscriptionType);

        var result = await _mediator.Send(command);

        return result.IsSuccess
            ? StatusCode(result.StatusCode, result.Data)
            : StatusCode(result.StatusCode, new { error = result.Error });
    }

    /// <summary>
    /// Endpoint apelat de Stripe pentru a notifica eventuri (plată reușită, anulare, etc.).
    /// IMPORTANT:
    ///  - Anonim (Stripe nu trimite JWT)
    ///  - Citim body-ul RAW (necesar pentru validarea HMAC a semnăturii)
    ///  - Răspundem 200 OK pentru orice eveniment procesat (chiar dacă-l ignorăm)
    ///    ca Stripe să nu retrimită inutil
    /// </summary>
    [HttpPost("webhook")]
    [AllowAnonymous]
    public async Task<IActionResult> StripeWebhook()
    {
        // Citim body-ul exact așa cum a venit, fără model binding.
        // Semnătura Stripe este HMAC peste payload-ul raw + timestamp.
        using var reader = new StreamReader(Request.Body);
        var payload = await reader.ReadToEndAsync();

        var signatureHeader = Request.Headers["Stripe-Signature"].ToString();

        if (string.IsNullOrWhiteSpace(signatureHeader))
        {
            return BadRequest(new { error = "Missing Stripe-Signature header." });
        }

        var command = new HandleStripeWebhookCommand(payload, signatureHeader);
        var result = await _mediator.Send(command);

        return result.IsSuccess
            ? Ok()
            : StatusCode(result.StatusCode, new { error = result.Error });
    }
}
