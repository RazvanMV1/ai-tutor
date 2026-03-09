using System.Net.Http.Json;
using System.Text.Json;
using AiTutor.Web.Models;

namespace AiTutor.Web.Services;

public class AiService
{
    private readonly AuthService _auth;

    private static JsonSerializerOptions JsonOpts => new()
    { PropertyNameCaseInsensitive = true };

    public AiService(AuthService auth) => _auth = auth;

    public async Task<ExplanationResponse?> GetExplanationAsync(
        ExplanationRequest request)
    {
        var client = await _auth.GetAuthenticatedClientAsync();
        var response = await client.PostAsJsonAsync(
            "/api/Ai/explanation", request);
        response.EnsureSuccessStatusCode();
        return await response.Content
            .ReadFromJsonAsync<ExplanationResponse>(JsonOpts);
    }

    public async Task<HintResponse?> GetHintAsync(HintRequest request)
    {
        var client = await _auth.GetAuthenticatedClientAsync();
        var response = await client.PostAsJsonAsync("/api/Ai/hint", request);
        response.EnsureSuccessStatusCode();
        return await response.Content
            .ReadFromJsonAsync<HintResponse>(JsonOpts);
    }

    public async Task<ProblemResponse?> GetProblemsAsync(ProblemRequest request)
    {
        var client = await _auth.GetAuthenticatedClientAsync();
        var response = await client.PostAsJsonAsync("/api/Ai/problems", request);
        response.EnsureSuccessStatusCode();
        return await response.Content
            .ReadFromJsonAsync<ProblemResponse>(JsonOpts);
    }
}
