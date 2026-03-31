using System.Net;
using System.Text.Json;
using AiTutor.IntegrationTests.Setup;
using FluentAssertions;
using Xunit;

namespace AiTutor.IntegrationTests.Controllers;

public class LessonsControllerTests : BaseIntegrationTest
{
    public LessonsControllerTests(IntegrationTestWebAppFactory factory) : base(factory) { }

    private async Task<string> GetSubjectIdAsync()
    {
        var response = await Client.GetAsync("/api/Subjects");
        var body = await response.Content.ReadAsStringAsync();
        var subjects = JsonSerializer.Deserialize<JsonElement>(body, JsonOptions);
        return subjects[0].GetProperty("id").GetString()!;
    }

    private async Task SetupTeacherAuthAsync()
    {
        var email = $"lessonteach{Guid.NewGuid():N}@test.com";
        var token = await GetAuthTokenAsync(email, role: 3); // Teacher = 3
        SetAuthHeader(token);
    }

    [Fact]
    public async Task GetBySubjectId_WithAuth_ShouldReturnLessons()
    {
        await SetupTeacherAuthAsync();
        var subjectId = await GetSubjectIdAsync();

        var response = await Client.GetAsync($"/api/Lessons/subject/{subjectId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Create_AsTeacher_ShouldReturn201()
    {
        await SetupTeacherAuthAsync();
        var subjectId = await GetSubjectIdAsync();

        var content = CreateJsonContent(new
        {
            title = "Integration Test Lesson",
            content = "Lesson content for testing",
            orderIndex = 1,
            difficulty = 1,
            subjectId
        });

        var response = await Client.PostAsync("/api/Lessons", content);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var body = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<JsonElement>(body, JsonOptions);
        data.GetProperty("title").GetString().Should().Be("Integration Test Lesson");
    }

    [Fact]
    public async Task Create_AsStudent_ShouldReturn403()
    {
        var email = $"lessonstudent{Guid.NewGuid():N}@test.com";
        var token = await GetAuthTokenAsync(email, role: 1); // Student = 1
        SetAuthHeader(token);
        var subjectId = await GetSubjectIdAsync();

        var content = CreateJsonContent(new
        {
            title = "Lesson",
            content = "Content",
            orderIndex = 1,
            difficulty = 1,
            subjectId
        });

        var response = await Client.PostAsync("/api/Lessons", content);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Create_WithoutAuth_ShouldReturn401()
    {
        Client.DefaultRequestHeaders.Authorization = null;

        var content = CreateJsonContent(new
        {
            title = "Lesson",
            content = "Content",
            orderIndex = 1,
            difficulty = 1,
            subjectId = Guid.NewGuid()
        });

        var response = await Client.PostAsync("/api/Lessons", content);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetById_WithValidId_ShouldReturnLesson()
    {
        await SetupTeacherAuthAsync();
        var subjectId = await GetSubjectIdAsync();

        var createContent = CreateJsonContent(new
        {
            title = "GetById Lesson",
            content = "Content",
            orderIndex = 1,
            difficulty = 1,
            subjectId
        });
        var createResponse = await Client.PostAsync("/api/Lessons", createContent);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var createBody = await createResponse.Content.ReadAsStringAsync();
        var created = JsonSerializer.Deserialize<JsonElement>(createBody, JsonOptions);
        var lessonId = created.GetProperty("id").GetString();

        var response = await Client.GetAsync($"/api/Lessons/{lessonId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        var lesson = JsonSerializer.Deserialize<JsonElement>(body, JsonOptions);
        lesson.GetProperty("title").GetString().Should().Be("GetById Lesson");
    }

    [Fact]
    public async Task GetById_WithInvalidId_ShouldReturn404()
    {
        await SetupTeacherAuthAsync();

        var response = await Client.GetAsync($"/api/Lessons/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
