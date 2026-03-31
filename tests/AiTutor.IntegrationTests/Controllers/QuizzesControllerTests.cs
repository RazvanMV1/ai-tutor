using System.Net;
using System.Text.Json;
using AiTutor.IntegrationTests.Setup;
using FluentAssertions;
using Xunit;

namespace AiTutor.IntegrationTests.Controllers;

public class QuizzesControllerTests : BaseIntegrationTest
{
    public QuizzesControllerTests(IntegrationTestWebAppFactory factory) : base(factory) { }

    private async Task SetupTeacherAuthAsync()
    {
        var email = $"quizteach{Guid.NewGuid():N}@test.com";
        var token = await GetAuthTokenAsync(email, role: 3); // Teacher = 3
        SetAuthHeader(token);
    }

    private async Task<string> GetSubjectIdAsync()
    {
        var response = await Client.GetAsync("/api/Subjects");
        var body = await response.Content.ReadAsStringAsync();
        var subjects = JsonSerializer.Deserialize<JsonElement>(body, JsonOptions);
        return subjects[0].GetProperty("id").GetString()!;
    }

    private async Task<string> CreateLessonAsync(string subjectId)
    {
        var content = CreateJsonContent(new
        {
            title = $"Quiz Lesson {Guid.NewGuid():N}",
            content = "Content",
            orderIndex = 1,
            difficulty = 1,
            subjectId
        });
        var response = await Client.PostAsync("/api/Lessons", content);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var body = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<JsonElement>(body, JsonOptions);
        return data.GetProperty("id").GetString()!;
    }

    private async Task<string> CreateQuizAsync(string lessonId, string title = "Test Quiz")
    {
        var content = CreateJsonContent(new
        {
            title,
            difficulty = 1,
            lessonId,
            timeLimitMinutes = 30
        });
        var response = await Client.PostAsync("/api/Quizzes", content);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var body = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<JsonElement>(body, JsonOptions);
        return data.GetProperty("id").GetString()!;
    }

    [Fact]
    public async Task Create_AsTeacher_ShouldReturn201()
    {
        await SetupTeacherAuthAsync();
        var subjectId = await GetSubjectIdAsync();
        var lessonId = await CreateLessonAsync(subjectId);

        var content = CreateJsonContent(new
        {
            title = "Integration Quiz",
            difficulty = 1,
            lessonId,
            timeLimitMinutes = 30
        });

        var response = await Client.PostAsync("/api/Quizzes", content);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var body = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<JsonElement>(body, JsonOptions);
        data.GetProperty("title").GetString().Should().Be("Integration Quiz");
    }

    [Fact]
    public async Task GetByLesson_ShouldReturnQuizzes()
    {
        await SetupTeacherAuthAsync();
        var subjectId = await GetSubjectIdAsync();
        var lessonId = await CreateLessonAsync(subjectId);
        await CreateQuizAsync(lessonId, "ByLesson Quiz");

        var response = await Client.GetAsync($"/api/Quizzes/lesson/{lessonId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        var quizzes = JsonSerializer.Deserialize<JsonElement>(body, JsonOptions);
        quizzes.GetArrayLength().Should().BeGreaterOrEqualTo(1);
    }

    [Fact]
    public async Task GetById_WithValidId_ShouldReturnQuiz()
    {
        await SetupTeacherAuthAsync();
        var subjectId = await GetSubjectIdAsync();
        var lessonId = await CreateLessonAsync(subjectId);
        var quizId = await CreateQuizAsync(lessonId, "GetById Quiz");

        var response = await Client.GetAsync($"/api/Quizzes/{quizId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task AddQuestion_ShouldReturn201()
    {
        await SetupTeacherAuthAsync();
        var subjectId = await GetSubjectIdAsync();
        var lessonId = await CreateLessonAsync(subjectId);
        var quizId = await CreateQuizAsync(lessonId, "Question Quiz");

        var questionContent = CreateJsonContent(new
        {
            text = "What is 2+2?",
            correctAnswer = "4",
            options = new[] { "2", "3", "4", "5" },
            points = 10,
            quizId,
            explanation = "Basic math"
        });

        var response = await Client.PostAsync($"/api/Quizzes/{quizId}/questions", questionContent);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Submit_WithAnswers_ShouldReturnResult()
    {
        await SetupTeacherAuthAsync();
        var subjectId = await GetSubjectIdAsync();
        var lessonId = await CreateLessonAsync(subjectId);
        var quizId = await CreateQuizAsync(lessonId, "Submit Quiz");

        // Adaugă întrebare
        var questionContent = CreateJsonContent(new
        {
            text = "What is 3+3?",
            correctAnswer = "6",
            options = new[] { "4", "5", "6", "7" },
            points = 10,
            quizId,
            explanation = "Addition"
        });
        var qResponse = await Client.PostAsync($"/api/Quizzes/{quizId}/questions", questionContent);
        qResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var qBody = await qResponse.Content.ReadAsStringAsync();
        var qData = JsonSerializer.Deserialize<JsonElement>(qBody, JsonOptions);
        var questionId = qData.GetProperty("id").GetString();

        // Submit quiz
        var submitContent = CreateJsonContent(new
        {
            quizId,
            userId = Guid.NewGuid(),
            answers = new[] { new { questionId, answer = "6" } }
        });

        var response = await Client.PostAsync($"/api/Quizzes/{quizId}/submit", submitContent);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<JsonElement>(body, JsonOptions);
        result.GetProperty("scorePercentage").GetInt32().Should().Be(100);
    }

    [Fact]
    public async Task Create_WithoutAuth_ShouldReturn401()
    {
        Client.DefaultRequestHeaders.Authorization = null;

        var content = CreateJsonContent(new
        {
            title = "Quiz",
            difficulty = 1,
            lessonId = Guid.NewGuid(),
            timeLimitMinutes = 30
        });

        var response = await Client.PostAsync("/api/Quizzes", content);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
