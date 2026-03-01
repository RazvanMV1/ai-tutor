using System.Net.Http;
using System.Net.Http.Json;
using AiTutor.Web.Models;
using Microsoft.JSInterop;

namespace AiTutor.Web.Services;

public class ApiService
{
    private readonly IHttpClientFactory _factory;
    private readonly IJSRuntime _js;
    private HttpClient? _client;

    public ApiService(IHttpClientFactory factory, IJSRuntime js)
    {
        _factory = factory;
        _js = js;
    }

    private async Task<HttpClient> GetClientAsync()
    {
        if (_client is not null) return _client;
        _client = _factory.CreateClient("BackendApi");
        var token = await _js.InvokeAsync<string?>("localStorage.getItem", "auth_token");
        if (!string.IsNullOrEmpty(token))
            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        return _client;
    }

    public async Task<List<SubjectDto>> GetSubjectsAsync()
    {
        var client = await GetClientAsync();
        try
        {
            return await client.GetFromJsonAsync<List<SubjectDto>>("/api/subjects")
                   ?? new List<SubjectDto>();
        }
        catch { return new List<SubjectDto>(); }
    }

    public async Task<LessonDto?> GetLessonAsync(int id)
    {
        var client = await GetClientAsync();
        try { return await client.GetFromJsonAsync<LessonDto>($"/api/lessons/{id}"); }
        catch { return null; }
    }

    public async Task<List<StudentProgressDto>> GetProgressAsync(string userId)
    {
        var client = await GetClientAsync();
        try
        {
            return await client.GetFromJsonAsync<List<StudentProgressDto>>($"/api/progress/{userId}")
                   ?? new List<StudentProgressDto>();
        }
        catch { return new List<StudentProgressDto>(); }
    }

    public async Task<bool> CompleteProgressAsync(CompleteProgressRequest request)
    {
        var client = await GetClientAsync();
        try
        {
            var response = await client.PostAsJsonAsync("/api/progress/complete", request);
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<UserDto?> GetUserAsync(string id)
    {
        var client = await GetClientAsync();
        try { return await client.GetFromJsonAsync<UserDto>($"/api/users/{id}"); }
        catch { return null; }
    }
}
