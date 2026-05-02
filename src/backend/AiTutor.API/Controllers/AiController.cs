using AiTutor.Application.Features.AI.Queries.GenerateProblems;
using AiTutor.Application.Features.AI.Queries.GetExplanation;
using AiTutor.Application.Features.AI.Queries.GetHint;
using AiTutor.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Text.Json;

namespace AiTutor.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AiController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public AiController(IMediator mediator, IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _mediator = mediator;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

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

    public record ChatMessage(string Role, string Content);

    public record LessonChatRequest(
        string LessonTitle,
        string LessonContent,
        SubjectType Subject,
        DifficultyLevel DifficultyLevel,
        string Question,
        List<ChatMessage>? History
    );

    public record LessonChatResponse(string Answer);

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

    /// <summary>AI tutor chat about a specific lesson (with conversation history)</summary>
    [HttpPost("lesson-chat")]
    public async Task<IActionResult> LessonChat([FromBody] LessonChatRequest request)
    {
        try
        {
            var aiBaseUrl = _configuration["AiModule:BaseUrl"] ?? "http://ai-module:8000";
            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(60);

            // Forward to Python AI module with snake_case keys
            var payload = new
            {
                lesson_title = request.LessonTitle,
                lesson_content = request.LessonContent ?? string.Empty,
                subject = (int)request.Subject,
                difficulty_level = (int)request.DifficultyLevel,
                question = request.Question,
                history = (request.History ?? new List<ChatMessage>())
                    .Select(m => new { role = m.Role, content = m.Content })
                    .ToList()
            };

            var response = await client.PostAsJsonAsync($"{aiBaseUrl}/api/lesson-chat", payload);
            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, body);

            using var doc = JsonDocument.Parse(body);
            var answer = doc.RootElement.TryGetProperty("answer", out var a) ? a.GetString() ?? "" : "";
            return Ok(new LessonChatResponse(answer));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
