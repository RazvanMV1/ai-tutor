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

    // ============================================================
    // SUBSCRIPTIONS â€” Stripe integrated
    // ============================================================

    /// <summary>
    /// Backend-ul returneazÄƒ { "value": [...], "Count": N }.
    /// </summary>
    public async Task<List<SubscriptionDto>> GetUserSubscriptionsAsync(Guid userId)
    {
        await AttachTokenAsync();
        try
        {
            var content = await _httpClient.GetStringAsync($"api/Subscriptions/user/{userId}");
            // Backend returneazÄƒ direct un array JSON: [{ ... }, { ... }]
            return JsonSerializer.Deserialize<List<SubscriptionDto>>(content, _jsonOptions) ?? new();
        }
        catch
        {
            return new();
        }
    }


    /// <summary>
    /// Compatibility wrapper: returneazÄƒ prima subscripÈ›ie activÄƒ, sau prima din listÄƒ, sau null.
    /// </summary>
    public async Task<SubscriptionDto?> GetSubscriptionByUserAsync(Guid userId)
    {
        var list = await GetUserSubscriptionsAsync(userId);
        if (list.Count == 0) return null;
        return list.FirstOrDefault(s => s.IsActive) ?? list.FirstOrDefault();
    }

    /// <summary>
    /// Creates a Stripe Checkout Session and returns SessionId + CheckoutUrl.
    /// User-ul va fi redirectat la CheckoutUrl pentru a finaliza plata.
    /// </summary>
    public async Task<CheckoutSessionResponse?> CreateCheckoutSessionAsync(CreateCheckoutSessionRequest request)
    {
        await AttachTokenAsync();
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/Subscriptions/checkout-session", request);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<CheckoutSessionResponse>(content, _jsonOptions);
            }
            return null;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Legacy: creates a subscription directly (without Stripe). Kept for backward compatibility.
    /// </summary>
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

    // ===== Parent â†” Child =====
    public async Task<InvitationCodeDto?> GetInvitationCodeAsync(Guid parentId)
    {
        await AttachTokenAsync();
        try
        {
            var response = await _httpClient.GetAsync($"api/Users/{parentId}/invitation-code");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<InvitationCodeDto>(content, _jsonOptions);
            }
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<ChildDto>> GetChildrenAsync(Guid parentId)
    {
        await AttachTokenAsync();
        try
        {
            var response = await _httpClient.GetAsync($"api/Users/parent/{parentId}/children");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<ChildDto>>(content, _jsonOptions) ?? new();
            }
            return new();
        }
        catch
        {
            return new();
        }
    }

    public async Task<(bool Success, string Message, LinkParentResponse? Data)> LinkParentAsync(Guid studentId, string invitationCode)
    {
        await AttachTokenAsync();
        try
        {
            var request = new LinkParentRequest { StudentId = studentId, InvitationCode = invitationCode };
            var response = await _httpClient.PostAsJsonAsync("api/Users/link-parent", request);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var data = JsonSerializer.Deserialize<LinkParentResponse>(content, _jsonOptions);
                return (true, "Cont legat cu succes!", data);
            }

            try
            {
                using var doc = JsonDocument.Parse(content);
                if (doc.RootElement.TryGetProperty("message", out var msg))
                    return (false, msg.GetString() ?? "Eroare necunoscutÄƒ.", null);
            }
            catch { }
            return (false, "Cod invalid sau eroare server.", null);
        }
        catch
        {
            return (false, "Eroare de conexiune.", null);
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

    // Classrooms
    public async Task<List<ClassroomDto>> GetMyClassroomsAsync(Guid userId)
    {
        await AttachTokenAsync();
        try
        {
            var content = await _httpClient.GetStringAsync($"api/Classrooms/my?userId={userId}");
            return JsonSerializer.Deserialize<List<ClassroomDto>>(content, _jsonOptions) ?? new();
        }
        catch { return new(); }
    }

    public async Task<ClassroomDto?> GetClassroomByIdAsync(Guid id, Guid userId)
    {
        await AttachTokenAsync();
        try
        {
            var content = await _httpClient.GetStringAsync($"api/Classrooms/{id}?userId={userId}");
            return JsonSerializer.Deserialize<ClassroomDto>(content, _jsonOptions);
        }
        catch { return null; }
    }

    public async Task<ClassroomDto?> CreateClassroomAsync(CreateClassroomRequest request, Guid teacherId)
    {
        await AttachTokenAsync();
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Classrooms?teacherId={teacherId}", request);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ClassroomDto>(content, _jsonOptions);
            }
            return null;
        }
        catch { return null; }
    }

    public async Task<bool> DeleteClassroomAsync(Guid id, Guid teacherId)
    {
        await AttachTokenAsync();
        try
        {
            var response = await _httpClient.DeleteAsync($"api/Classrooms/{id}?teacherId={teacherId}");
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<bool> JoinClassroomAsync(JoinClassroomRequest request)
    {
        await AttachTokenAsync();
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/Classrooms/join", request);
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<List<ClassroomMemberDto>> GetClassroomMembersAsync(Guid classroomId, Guid teacherId)
    {
        await AttachTokenAsync();
        try
        {
            var content = await _httpClient.GetStringAsync($"api/Classrooms/{classroomId}/members?teacherId={teacherId}");
            return JsonSerializer.Deserialize<List<ClassroomMemberDto>>(content, _jsonOptions) ?? new();
        }
        catch { return new(); }
    }

    public async Task<bool> AddMemberAsync(Guid classroomId, Guid teacherId, string studentEmail)
    {
        await AttachTokenAsync();
        try
        {
            var response = await _httpClient.PostAsJsonAsync(
                $"api/Classrooms/{classroomId}/members?teacherId={teacherId}",
                new { StudentEmail = studentEmail });
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<bool> RemoveMemberAsync(Guid classroomId, Guid studentId, Guid teacherId)
    {
        await AttachTokenAsync();
        try
        {
            var response = await _httpClient.DeleteAsync($"api/Classrooms/{classroomId}/members/{studentId}?teacherId={teacherId}");
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<List<ClassroomLessonDto>> GetClassroomLessonsAsync(Guid classroomId, Guid userId)
    {
        await AttachTokenAsync();
        try
        {
            var content = await _httpClient.GetStringAsync($"api/Classrooms/{classroomId}/lessons?userId={userId}");
            return JsonSerializer.Deserialize<List<ClassroomLessonDto>>(content, _jsonOptions) ?? new();
        }
        catch { return new(); }
    }

    public async Task<ClassroomLessonDto?> CreateClassroomLessonAsync(Guid classroomId, Guid teacherId, CreateClassroomLessonRequest request)
    {
        await AttachTokenAsync();
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Classrooms/{classroomId}/lessons?teacherId={teacherId}", request);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ClassroomLessonDto>(content, _jsonOptions);
            }
            return null;
        }
        catch { return null; }
    }

    public async Task<ClassroomLessonDto?> GetClassroomLessonByIdAsync(Guid classroomId, Guid lessonId, Guid userId)
    {
        await AttachTokenAsync();
        try
        {
            var content = await _httpClient.GetStringAsync(
                $"api/Classrooms/{classroomId}/lessons/{lessonId}?userId={userId}");
            return JsonSerializer.Deserialize<ClassroomLessonDto>(content, _jsonOptions);
        }
        catch
        {
            return null;
        }
    }

    public async Task<ClassroomLessonDto?> UpdateClassroomLessonAsync(
        Guid classroomId, Guid lessonId, Guid teacherId, UpdateClassroomLessonRequest request)
    {
        await AttachTokenAsync();
        try
        {
            var response = await _httpClient.PutAsJsonAsync(
                $"api/Classrooms/{classroomId}/lessons/{lessonId}?teacherId={teacherId}", request);
            if (!response.IsSuccessStatusCode) return null;
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ClassroomLessonDto>(content, _jsonOptions);
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> DeleteClassroomLessonAsync(Guid classroomId, Guid lessonId, Guid teacherId)
    {
        await AttachTokenAsync();
        try
        {
            var response = await _httpClient.DeleteAsync(
                $"api/Classrooms/{classroomId}/lessons/{lessonId}?teacherId={teacherId}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }



    public async Task<ClassroomQuizDto?> CreateClassroomQuizAsync(Guid classroomId, Guid lessonId, Guid teacherId, CreateClassroomQuizRequest request)
    {
        await AttachTokenAsync();
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Classrooms/{classroomId}/lessons/{lessonId}/quizzes?teacherId={teacherId}", request);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ClassroomQuizDto>(content, _jsonOptions);
            }
            return null;
        }
        catch { return null; }
    }

    public async Task<List<ClassroomQuizDto>> GetClassroomQuizzesByLessonAsync(Guid classroomId, Guid lessonId, Guid userId)
    {
        await AttachTokenAsync();
        try
        {
            var content = await _httpClient.GetStringAsync(
                $"api/Classrooms/{classroomId}/lessons/{lessonId}/quizzes?userId={userId}");
            return JsonSerializer.Deserialize<List<ClassroomQuizDto>>(content, _jsonOptions) ?? new();
        }
        catch
        {
            return new();
        }
    }

    public async Task<ClassroomQuizDetailDto?> GetClassroomQuizByIdAsync(Guid classroomId, Guid quizId, Guid userId)
    {
        await AttachTokenAsync();
        try
        {
            var content = await _httpClient.GetStringAsync(
                $"api/Classrooms/{classroomId}/quizzes/{quizId}?userId={userId}");
            return JsonSerializer.Deserialize<ClassroomQuizDetailDto>(content, _jsonOptions);
        }
        catch
        {
            return null;
        }
    }

    public async Task<ClassroomQuizDto?> UpdateClassroomQuizAsync(
        Guid classroomId, Guid quizId, Guid teacherId, UpdateClassroomQuizRequest request)
    {
        await AttachTokenAsync();
        try
        {
            var response = await _httpClient.PutAsJsonAsync(
                $"api/Classrooms/{classroomId}/quizzes/{quizId}?teacherId={teacherId}", request);
            if (!response.IsSuccessStatusCode) return null;
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ClassroomQuizDto>(content, _jsonOptions);
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> DeleteClassroomQuizAsync(Guid classroomId, Guid quizId, Guid teacherId)
    {
        await AttachTokenAsync();
        try
        {
            var response = await _httpClient.DeleteAsync(
                $"api/Classrooms/{classroomId}/quizzes/{quizId}?teacherId={teacherId}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteClassroomQuestionAsync(Guid classroomId, Guid quizId, Guid questionId, Guid teacherId)
    {
        await AttachTokenAsync();
        try
        {
            var response = await _httpClient.DeleteAsync(
                $"api/Classrooms/{classroomId}/quizzes/{quizId}/questions/{questionId}?teacherId={teacherId}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }


    public async Task<ClassroomQuestionDto?> AddClassroomQuestionAsync(Guid classroomId, Guid quizId, Guid teacherId, AddClassroomQuestionRequest request)
    {
        await AttachTokenAsync();
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Classrooms/{classroomId}/lessons/any/quizzes/{quizId}/questions?teacherId={teacherId}", request);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ClassroomQuestionDto>(content, _jsonOptions);
            }
            return null;
        }
        catch { return null; }
    }

    public async Task<ClassroomQuizResultResponse?> SubmitClassroomQuizAsync(Guid classroomId, Guid quizId, Guid studentId, SubmitClassroomQuizRequest request)
    {
        await AttachTokenAsync();
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Classrooms/{classroomId}/lessons/any/quizzes/{quizId}/submit?studentId={studentId}", request);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ClassroomQuizResultResponse>(content, _jsonOptions);
            }
            return null;
        }
        catch { return null; }
    }

    public async Task<List<ClassroomProgressDto>> GetClassroomProgressAsync(Guid classroomId, Guid userId)
    {
        await AttachTokenAsync();
        try
        {
            var content = await _httpClient.GetStringAsync($"api/Classrooms/{classroomId}/progress?userId={userId}");
            return JsonSerializer.Deserialize<List<ClassroomProgressDto>>(content, _jsonOptions) ?? new();
        }
        catch { return new(); }
    }

    public async Task<List<GradeDto>> GetClassroomGradesAsync(Guid classroomId, Guid userId, bool isTeacher)
    {
        await AttachTokenAsync();
        try
        {
            var content = await _httpClient.GetStringAsync($"api/Classrooms/{classroomId}/grades?userId={userId}&isTeacher={isTeacher}");
            return JsonSerializer.Deserialize<List<GradeDto>>(content, _jsonOptions) ?? new();
        }
        catch { return new(); }
    }

    public async Task<GradeDto?> AddGradeAsync(Guid classroomId, Guid teacherId, AddGradeRequest request)
    {
        await AttachTokenAsync();
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Classrooms/{classroomId}/grades?teacherId={teacherId}", request);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<GradeDto>(content, _jsonOptions);
            }
            return null;
        }
        catch { return null; }
    }

    public async Task<bool> DeleteGradeAsync(Guid classroomId, Guid gradeId, Guid teacherId)
    {
        await AttachTokenAsync();
        try
        {
            var response = await _httpClient.DeleteAsync($"api/Classrooms/{classroomId}/grades/{gradeId}?teacherId={teacherId}");
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<ParentInfoDto?> GetMyParentAsync(Guid studentId)
    {
        await AttachTokenAsync();
        try
        {
            var response = await _httpClient.GetAsync($"api/Users/{studentId}/my-parent");
            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(content) || content == "null") return null;
            return JsonSerializer.Deserialize<ParentInfoDto>(content, _jsonOptions);
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<StudentGradeDto>> GetStudentGradesAsync(Guid studentId, Guid requesterId)
    {
        await AttachTokenAsync();
        try
        {
            var response = await _httpClient.GetAsync($"api/Users/student/{studentId}/grades?requesterId={requesterId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<StudentGradeDto>>(content, _jsonOptions) ?? new();
            }
            return new();
        }
        catch
        {
            return new();
        }
    }


    public async Task<bool> UnlinkParentAsync(Guid studentId)
    {
        await AttachTokenAsync();
        try
        {
            var response = await _httpClient.DeleteAsync($"api/Users/{studentId}/my-parent");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }



    // ===== AI Tutor — Lesson Chat =====
    public async Task<LessonChatResponse?> AskLessonAiAsync(LessonChatRequest request)
    {
        await AttachTokenAsync();
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/Ai/lesson-chat", request);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<LessonChatResponse>(content, _jsonOptions);
            }
            return null;
        }
        catch
        {
            return null;
        }
    }
}
