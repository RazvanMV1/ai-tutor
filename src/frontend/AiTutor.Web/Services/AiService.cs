using System.Net.Http.Json;
using System.Text.Json;
using AiTutor.Web.Models;

namespace AiTutor.Web.Services;

public class AiService
{
    private readonly IHttpClientFactory _httpFactory;

    public AiService(IHttpClientFactory httpFactory)
        => _httpFactory = httpFactory;

    private HttpClient Client => _httpFactory.CreateClient("AiApi");

    private static JsonSerializerOptions JsonOpts => new()
    { PropertyNameCaseInsensitive = true };

    public async Task<ExplanationResponse?> GetExplanationAsync(ExplanationRequest request)
    {
        var response = await Client.PostAsJsonAsync("/api/explanations", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ExplanationResponse>(JsonOpts);
    }

    public async Task<HintResponse?> GetHintAsync(HintRequest request)
    {
        var response = await Client.PostAsJsonAsync("/api/explanations/hint", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<HintResponse>(JsonOpts);
    }

    public async Task<ProblemResponse?> GetProblemsAsync(ProblemRequest request)
    {
        var response = await Client.PostAsJsonAsync("/api/problems", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ProblemResponse>(JsonOpts);
    }
}
