using System.Net.Http.Json;
using System.Text.Json;
using AiTutor.Web.Models;

namespace AiTutor.Web.Services;

public class ApiService
{
    private readonly AuthService _auth;

    private static JsonSerializerOptions JsonOpts => new()
    { PropertyNameCaseInsensitive = true };

    public ApiService(AuthService auth) => _auth = auth;

    // ── Auth ─────────────────────────────────────────────────────────────────
    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var client = _auth.GetUnauthenticatedClient();
        var response = await client.PostAsJsonAsync("/api/auth/login", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<AuthResponse>(JsonOpts);
    }

    public async Task RegisterAsync(RegisterRequest request)
    {
        var client = _auth.GetUnauthenticatedClient();
        var response = await client.PostAsJsonAsync("/api/auth/register", request);
        response.EnsureSuccessStatusCode();
    }

    // ── Subjects ─────────────────────────────────────────────────────────────
    public async Task<List<SubjectDto>> GetSubjectsAsync()
    {
        var client = await _auth.GetAuthenticatedClientAsync();
        return await client.GetFromJsonAsync<List<SubjectDto>>("/api/subjects", JsonOpts)
               ?? [];
    }

    // ── Lessons ──────────────────────────────────────────────────────────────
    public async Task<LessonDto?> GetLessonAsync(Guid id)
    {
        var client = await _auth.GetAuthenticatedClientAsync();
        return await client.GetFromJsonAsync<LessonDto>($"/api/lessons/{id}", JsonOpts);
    }

    // ── Progress ─────────────────────────────────────────────────────────────
    public async Task<List<StudentProgressDto>> GetProgressAsync(Guid userId)
    {
        var client = await _auth.GetAuthenticatedClientAsync();
        return await client.GetFromJsonAsync<List<StudentProgressDto>>(
            $"/api/progress/{userId}", JsonOpts) ?? [];
    }

    public async Task CompleteLessonAsync(CompleteLessonRequest request)
    {
        var client = await _auth.GetAuthenticatedClientAsync();
        var response = await client.PostAsJsonAsync("/api/progress/complete", request);
        response.EnsureSuccessStatusCode();
    }
}
