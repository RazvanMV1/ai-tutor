using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text.Json;
using AiTutor.Web.Models;

namespace AiTutor.Web.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly AuthService _authService;

    // JSON options: case-insensitive property matching, number-to-enum support
    private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString,
    };

    public ApiService(HttpClient httpClient, AuthService authService)
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

    // Auth
    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/Auth/login", request);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<LoginResponse>(content, _jsonOptions);
            }
            return null;
        }
        catch
        {
            throw; // Let the caller handle it
        }
    }

    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Auth/register", request);
        return response.IsSuccessStatusCode;
    }

    // Subjects
    public async Task<List<SubjectDto>> GetSubjectsAsync()
    {
        await AttachTokenAsync();
        try
        {
            var content = await _httpClient.GetStringAsync("api/Subjects");
            return JsonSerializer.Deserialize<List<SubjectDto>>(content, _jsonOptions) ?? new();
        }
        catch
        {
            return new();
        }
    }

    public async Task<SubjectDto?> CreateSubjectAsync(CreateSubjectRequest request)
    {
        await AttachTokenAsync();
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/Subjects", request);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<SubjectDto>(content, _jsonOptions);
            }
            return null;
        }
        catch
        {
            return null;
        }
    }

    // Lessons
    public async Task<LessonDto?> GetLessonByIdAsync(Guid id)
    {
        await AttachTokenAsync();
        try
        {
            var content = await _httpClient.GetStringAsync($"api/Lessons/{id}");
            return JsonSerializer.Deserialize<LessonDto>(content, _jsonOptions);
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<LessonDto>> GetLessonsBySubjectAsync(Guid subjectId)
    {
        await AttachTokenAsync();
        try
        {
            var content = await _httpClient.GetStringAsync($"api/Lessons/subject/{subjectId}");
            return JsonSerializer.Deserialize<List<LessonDto>>(content, _jsonOptions) ?? new();
        }
        catch
        {
            return new();
        }
    }

    public async Task<LessonDto?> CreateLessonAsync(CreateLessonRequest request)
    {
        await AttachTokenAsync();
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/Lessons", request);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<LessonDto>(content, _jsonOptions);
            }
            return null;
        }
        catch
        {
            return null;
        }
    }

    // Quizzes
    public async Task<QuizDetailDto?> GetQuizByIdAsync(Guid id)
    {
        await AttachTokenAsync();
        try
        {
            var content = await _httpClient.GetStringAsync($"api/Quizzes/{id}");
            return JsonSerializer.Deserialize<QuizDetailDto>(content, _jsonOptions);
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<QuizSummaryDto>> GetQuizzesByLessonAsync(Guid lessonId)
    {
        await AttachTokenAsync();
        try
        {
            var content = await _httpClient.GetStringAsync($"api/Quizzes/lesson/{lessonId}");
            return JsonSerializer.Deserialize<List<QuizSummaryDto>>(content, _jsonOptions) ?? new();
        }
        catch
        {
            return new();
        }
    }

    public async Task<QuizResultResponse?> SubmitQuizAsync(Guid quizId, SubmitQuizCommand command)
    {
        await AttachTokenAsync();
        command.QuizId = quizId;
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Quizzes/{quizId}/submit", command);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<QuizResultResponse>(content, _jsonOptions);
            }
            return null;
        }
        catch
        {
            return null;
        }
    }

    // Progress
    public async Task<List<StudentProgressDto>> GetProgressAsync(Guid userId)
    {
        await AttachTokenAsync();
        try
        {
            var content = await _httpClient.GetStringAsync($"api/Progress/{userId}");
            return JsonSerializer.Deserialize<List<StudentProgressDto>>(content, _jsonOptions) ?? new();
        }
        catch
        {
            return new();
        }
    }

    public async Task<bool> CompleteLessonAsync(CompleteLessonCommand command)
    {
        await AttachTokenAsync();
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/Progress/complete", command);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    // Subscriptions
    public async Task<SubscriptionDto?> GetSubscriptionByUserAsync(Guid userId)
    {
        await AttachTokenAsync();
        try
        {
            var content = await _httpClient.GetStringAsync($"api/Subscriptions/user/{userId}");
            return JsonSerializer.Deserialize<SubscriptionDto>(content, _jsonOptions);
        }
        catch
        {
            return null;
        }
    }

    public async Task<SubscriptionDto?> CreateSubscriptionAsync(CreateSubscriptionRequest request)
    {
        await AttachTokenAsync();
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/Subscriptions", request);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<SubscriptionDto>(content, _jsonOptions);
            }
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> CancelSubscriptionAsync(Guid subscriptionId, Guid userId)
    {
        await AttachTokenAsync();
        try
        {
            var response = await _httpClient.DeleteAsync($"api/Subscriptions/{subscriptionId}?userId={userId}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
