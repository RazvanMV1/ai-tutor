using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text.Json;
using AiTutor.Web.Models;

namespace AiTutor.Web.Services;

public class AiService
{
    private readonly HttpClient _httpClient;
    private readonly AuthService _authService;

    private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString,
    };

    public AiService(HttpClient httpClient, AuthService authService)
    {
        _httpClient = httpClient;
        _authService = authService;
    }

    private async Task AttachTokenAsync()
    {
        var token = await _authService.GetTokenAsync();
        if (!string.IsNullOrEmpty(token))
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<ExplanationResponse?> GetExplanationAsync(ExplanationRequest request)
    {
        await AttachTokenAsync();
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/Ai/explanation", request);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ExplanationResponse>(content, _jsonOptions);
            }
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<HintResponse?> GetHintAsync(HintRequest request)
    {
        await AttachTokenAsync();
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/Ai/hint", request);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<HintResponse>(content, _jsonOptions);
            }
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<ProblemResponse?> GenerateProblemsAsync(ProblemsRequest request)
    {
        await AttachTokenAsync();
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/Ai/problems", request);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ProblemResponse>(content, _jsonOptions);
            }
            return null;
        }
        catch
        {
            return null;
        }
    }
}
