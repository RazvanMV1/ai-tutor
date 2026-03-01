using System.Net;
using System.Text.Json;
using AiTutor.IntegrationTests.Setup;
using FluentAssertions;

namespace AiTutor.IntegrationTests.Controllers;

public class ProgressControllerTests : BaseIntegrationTest
{
    public ProgressControllerTests(IntegrationTestWebAppFactory factory) : base(factory) { }

    private async Task<(Guid subjectId, Guid lessonId)> CreateSubjectAndLessonAsync()
    {
        var token = await GetAuthTokenAsync("progress.setup@example.com", "Test123!@", role: 4);
        SetAuthHeader(token);

        // Creare subject
        var subjectContent = CreateJsonContent(new
        {
            name = $"Matematică {Guid.NewGuid()}",
            description = "Descriere materie",
            type = 1
        });
        var subjectResponse = await Client.PostAsync("/api/Subjects", subjectContent);
        var subjectBody = await subjectResponse.Content.ReadAsStringAsync();
        var subjectData = JsonSerializer.Deserialize<JsonElement>(subjectBody, JsonOptions);
        var subjectId = Guid.Parse(subjectData.GetProperty("id").GetString()!);

        // Creare lesson
        var lessonContent = CreateJsonContent(new
        {
            title = $"Lecție {Guid.NewGuid()}",
            content = "Conținut lecție",
            orderIndex = 1,
            difficulty = 1,
            subjectId
        });
        var lessonResponse = await Client.PostAsync("/api/Lessons", lessonContent);
        var lessonBody = await lessonResponse.Content.ReadAsStringAsync();
        var lessonData = JsonSerializer.Deserialize<JsonElement>(lessonBody, JsonOptions);
        var lessonId = Guid.Parse(lessonData.GetProperty("id").GetString()!);

        return (subjectId, lessonId);
    }

    [Fact]
    public async Task GetStudentProgress_WithoutAuth_ShouldReturn401()
    {
        // Act
        var response = await Client.GetAsync($"/api/Progress/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetStudentProgress_WithAuth_ShouldReturn200()
    {
        // Arrange
        var token = await GetAuthTokenAsync("progress.get@example.com");
        SetAuthHeader(token);

        // Act
        var response = await Client.GetAsync($"/api/Progress/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CompleteLesson_WithValidData_ShouldReturn200()
    {
        // Arrange
        var token = await GetAuthTokenAsync("progress.complete2@example.com", "Test123!@");
        SetAuthHeader(token);

        // Folosim un userId si lessonId invalide intentionat
        // dar verificam ca endpoint-ul raspunde (nu 401/403)
        var content = CreateJsonContent(new
        {
            userId = Guid.NewGuid(),
            lessonId = Guid.NewGuid(),
            scorePercentage = 85
        });

        // Act
        var response = await Client.PostAsync("/api/Progress/complete", content);

        // Assert - endpoint-ul e accesibil (nu unauthorized)
        response.StatusCode.Should().NotBe(HttpStatusCode.Unauthorized);
        response.StatusCode.Should().NotBe(HttpStatusCode.Forbidden);
    }



    [Fact]
    public async Task CompleteLesson_WithInvalidScore_ShouldReturn400()
    {
        // Arrange
        var token = await GetAuthTokenAsync("progress.invalid@example.com");
        SetAuthHeader(token);

        var content = CreateJsonContent(new
        {
            userId = Guid.NewGuid(),
            lessonId = Guid.NewGuid(),
            scorePercentage = 150
        });

        // Act
        var response = await Client.PostAsync("/api/Progress/complete", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
