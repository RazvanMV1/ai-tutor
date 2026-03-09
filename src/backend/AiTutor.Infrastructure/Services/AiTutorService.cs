using System.Text;
using System.Text.Json;
using AiTutor.Application.Common.Interfaces;
using AiTutor.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AiTutor.Infrastructure.Services;

public class AiTutorService : IAiTutorService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AiTutorService> _logger;
    private readonly string _baseUrl;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        PropertyNameCaseInsensitive = true
    };

    public AiTutorService(HttpClient httpClient, IConfiguration configuration, ILogger<AiTutorService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _baseUrl = configuration["AiModule:BaseUrl"] ?? "http://ai-module:8000";
    }

    public async Task<ExplanationResponse> GetExplanationAsync(ExplanationRequest request, CancellationToken ct = default)
    {
        var payload = new
        {
            topic = request.Topic,
            subject = (int)request.Subject,
            difficulty_level = (int)request.DifficultyLevel,
            student_age = request.StudentAge
        };

        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        _logger.LogInformation("Calling AI module for explanation: {Topic}", request.Topic);

        var response = await _httpClient.PostAsync($"{_baseUrl}/api/explanations", content, ct);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync(ct);
        var result = JsonSerializer.Deserialize<AiExplanationResponse>(responseJson, JsonOptions)
            ?? throw new InvalidOperationException("Invalid response from AI module.");

        return new ExplanationResponse(
            result.Topic,
            result.Explanation,
            result.Examples,
            result.KeyPoints,
            request.Subject,
            request.DifficultyLevel
        );
    }

    public async Task<HintResponse> GetHintAsync(HintRequest request, CancellationToken ct = default)
    {
        var payload = new
        {
            question = request.Question,
            subject = (int)request.Subject
        };

        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        _logger.LogInformation("Calling AI module for hint");

        var response = await _httpClient.PostAsync($"{_baseUrl}/api/explanations/hint", content, ct);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync(ct);
        var result = JsonSerializer.Deserialize<AiHintResponse>(responseJson, JsonOptions)
            ?? throw new InvalidOperationException("Invalid response from AI module.");

        return new HintResponse(result.Question, result.Hint, request.Subject);
    }

    public async Task<ProblemResponse> GenerateProblemsAsync(ProblemRequest request, CancellationToken ct = default)
    {
        var payload = new
        {
            topic = request.Topic,
            subject = (int)request.Subject,
            difficulty_level = (int)request.DifficultyLevel,
            student_age = request.StudentAge,
            count = request.Count
        };

        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        _logger.LogInformation("Calling AI module for problems: {Topic}", request.Topic);

        var response = await _httpClient.PostAsync($"{_baseUrl}/api/problems", content, ct);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync(ct);
        var result = JsonSerializer.Deserialize<AiProblemResponse>(responseJson, JsonOptions)
            ?? throw new InvalidOperationException("Invalid response from AI module.");

        return new ProblemResponse(
            result.Topic,
            result.Problems,
            result.Hints,
            result.Solutions,
            request.Subject,
            request.DifficultyLevel
        );
    }

    // Internal DTOs for AI module responses
    private record AiExplanationResponse(
        string Topic,
        string Explanation,
        List<string> Examples,
        List<string> KeyPoints
    );

    private record AiHintResponse(
        string Question,
        string Hint
    );

    private record AiProblemResponse(
        string Topic,
        List<string> Problems,
        List<string> Hints,
        List<string> Solutions
    );
}
