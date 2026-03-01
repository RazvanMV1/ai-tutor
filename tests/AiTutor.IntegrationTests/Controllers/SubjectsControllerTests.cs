using System.Net;
using System.Text.Json;
using AiTutor.IntegrationTests.Setup;
using FluentAssertions;

namespace AiTutor.IntegrationTests.Controllers;

public class SubjectsControllerTests : BaseIntegrationTest
{
    public SubjectsControllerTests(IntegrationTestWebAppFactory factory) : base(factory) { }

    [Fact]
    public async Task GetAll_WithoutAuth_ShouldReturn401()
    {
        // Act
        var response = await Client.GetAsync("/api/Subjects");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetAll_WithAuth_ShouldReturn200()
    {
        // Arrange
        var token = await GetAuthTokenAsync("subjects.test@example.com");
        SetAuthHeader(token);

        // Act
        var response = await Client.GetAsync("/api/Subjects");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Create_WithValidData_ShouldReturn201()
    {
        // Arrange
        var token = await GetAuthTokenAsync("subjects.create@example.com", "Test123!@", role: 4);
        SetAuthHeader(token);

        var content = CreateJsonContent(new
        {
            name = "Matematicã",
            description = "Materie de matematicã",
            type = 1
        });

        // Act
        var response = await Client.PostAsync("/api/Subjects", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }


    [Fact]
    public async Task Create_WithEmptyName_ShouldReturn400()
    {
        // Arrange
        var token = await GetAuthTokenAsync("subjects.invalid@example.com", "Test123!@", role: 4);
        SetAuthHeader(token);

        var content = CreateJsonContent(new
        {
            name = "",
            description = "Descriere",
            type = 1
        });

        // Act
        var response = await Client.PostAsync("/api/Subjects", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

}
