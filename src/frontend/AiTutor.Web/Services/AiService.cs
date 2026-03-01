using System.Net.Http;
using System.Net.Http.Json;
using AiTutor.Web.Models;
using Microsoft.JSInterop;

namespace AiTutor.Web.Services;

public class AiService
{
    private readonly IHttpClientFactory _factory;
    private readonly IJSRuntime _js;
    private HttpClient? _client;

    public AiService(IHttpClientFactory factory, IJSRuntime js)
    {
        _factory = factory;
        _js = js;
    }

    private async Task<HttpClient> GetClientAsync()
    {
        if (_client is not null) return _client;
        _client = _factory.CreateClient("AiApi");
        var token = await _js.InvokeAsync<string?>("localStorage.getItem", "auth_token");
        if (!string.IsNullOrEmpty(token))
            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        return _client;
    }

    public async Task<(bool Success, AiExplanationResponse? Data, string? Error)>
        GetExplanationAsync(AiExplanationRequest request)
    {
        var client = await GetClientAsync();
        try
        {
            var response = await client.PostAsJsonAsync("/api/explanations", request);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<AiExplanationResponse>();
                return (true, data, null);
            }
            return (false, null, $"Eroare AI: {(int)response.StatusCode}");
        }
        catch (Exception ex) { return (false, null, ex.Message); }
    }

    public async Task<(bool Success, AiExplanationResponse? Data, string? Error)>
        GetHintAsync(AiHintRequest request)
    {
        var client = await GetClientAsync();
        try
        {
            var response = await client.PostAsJsonAsync("/api/explanations/hint", request);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<AiExplanationResponse>();
                return (true, data, null);
            }
            return (false, null, $"Eroare AI: {(int)response.StatusCode}");
        }
        catch (Exception ex) { return (false, null, ex.Message); }
    }

    public async Task<(bool Success, AiProblemsResponse? Data, string? Error)>
        GetProblemsAsync(AiProblemsRequest request)
    {
        var client = await GetClientAsync();
        try
        {
            var response = await client.PostAsJsonAsync("/api/problems", request);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<AiProblemsResponse>();
                return (true, data, null);
            }
            return (false, null, $"Eroare AI: {(int)response.StatusCode}");
        }
        catch (Exception ex) { return (false, null, ex.Message); }
    }

    public async Task<bool> CheckHealthAsync()
    {
        var client = await GetClientAsync();
        try
        {
            var response = await client.GetAsync("/health");
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }
}
