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

    // ── Auth ──────────────────────────────────────────────────────────────────
    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var client = _auth.GetUnauthenticatedClient();
        var response = await client.PostAsJsonAsync("/api/Auth/login", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<AuthResponse>(JsonOpts);
    }

    public async Task<RegisterResponse?> RegisterAsync(RegisterRequest request)
    {
        var client = _auth.GetUnauthenticatedClient();
        var response = await client.PostAsJsonAsync("/api/Auth/register", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<RegisterResponse>(JsonOpts);
    }

    // ── Subjects ──────────────────────────────────────────────────────────────
    public async Task<List<SubjectDto>> GetSubjectsAsync()
    {
        var client = await _auth.GetAuthenticatedClientAsync();
        return await client.GetFromJsonAsync<List<SubjectDto>>(
            "/api/Subjects", JsonOpts) ?? [];
    }

    public async Task<SubjectDto?> CreateSubjectAsync(CreateSubjectRequest request)
    {
        var client = await _auth.GetAuthenticatedClientAsync();
        var response = await client.PostAsJsonAsync("/api/Subjects", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<SubjectDto>(JsonOpts);
    }

    // ── Lessons ───────────────────────────────────────────────────────────────
    public async Task<LessonDto?> GetLessonAsync(Guid id)
    {
        var client = await _auth.GetAuthenticatedClientAsync();
        return await client.GetFromJsonAsync<LessonDto>(
            $"/api/Lessons/{id}", JsonOpts);
    }

    public async Task<LessonDto?> CreateLessonAsync(CreateLessonRequest request)
    {
        var client = await _auth.GetAuthenticatedClientAsync();
        var response = await client.PostAsJsonAsync("/api/Lessons", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<LessonDto>(JsonOpts);
    }

    // ── Quizzes ───────────────────────────────────────────────────────────────
    public async Task<QuizDto?> GetQuizAsync(Guid id)
    {
        var client = await _auth.GetAuthenticatedClientAsync();
        return await client.GetFromJsonAsync<QuizDto>(
            $"/api/Quizzes/{id}", JsonOpts);
    }

    public async Task<QuizResultDto?> SubmitQuizAsync(Guid quizId,
        SubmitQuizRequest request)
    {
        var client = await _auth.GetAuthenticatedClientAsync();
        var response = await client.PostAsJsonAsync(
            $"/api/Quizzes/{quizId}/submit", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<QuizResultDto>(JsonOpts);
    }

    // ── Progress ──────────────────────────────────────────────────────────────
    public async Task<List<StudentProgressDto>> GetProgressAsync(Guid userId)
    {
        var client = await _auth.GetAuthenticatedClientAsync();
        return await client.GetFromJsonAsync<List<StudentProgressDto>>(
            $"/api/Progress/{userId}", JsonOpts) ?? [];
    }

    public async Task CompleteLessonAsync(CompleteLessonRequest request)
    {
        var client = await _auth.GetAuthenticatedClientAsync();
        var response = await client.PostAsJsonAsync(
            "/api/Progress/complete", request);
        response.EnsureSuccessStatusCode();
    }

    // ── Subscriptions ─────────────────────────────────────────────────────────
    public async Task<List<SubscriptionDto>> GetUserSubscriptionsAsync(Guid userId)
    {
        var client = await _auth.GetAuthenticatedClientAsync();
        return await client.GetFromJsonAsync<List<SubscriptionDto>>(
            $"/api/Subscriptions/user/{userId}", JsonOpts) ?? [];
    }

    public async Task<SubscriptionDto?> CreateSubscriptionAsync(
        CreateSubscriptionRequest request)
    {
        var client = await _auth.GetAuthenticatedClientAsync();
        var response = await client.PostAsJsonAsync("/api/Subscriptions", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<SubscriptionDto>(JsonOpts);
    }

    // ── Lessons ───────────────────────────────────────────────────────────────────
    public async Task<List<LessonDto>> GetLessonsBySubjectAsync(Guid subjectId)
    {
        var client = await _auth.GetAuthenticatedClientAsync();
        return await client.GetFromJsonAsync<List<LessonDto>>(
            $"/api/Lessons/subject/{subjectId}", JsonOpts) ?? [];
    }

    // ── Quizzes ───────────────────────────────────────────────────────────────────
    public async Task<List<QuizDto>> GetQuizzesByLessonAsync(Guid lessonId)
    {
        var client = await _auth.GetAuthenticatedClientAsync();
        return await client.GetFromJsonAsync<List<QuizDto>>(
            $"/api/Quizzes/lesson/{lessonId}", JsonOpts) ?? [];
    }

    public async Task CancelSubscriptionAsync(Guid subscriptionId, Guid userId)
    {
        var client = await _auth.GetAuthenticatedClientAsync();
        var response = await client.DeleteAsync(
            $"/api/Subscriptions/{subscriptionId}?userId={userId}");
        response.EnsureSuccessStatusCode();
    }

}
