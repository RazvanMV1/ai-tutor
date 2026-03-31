using System.Net;
using System.Text.Json;
using AiTutor.IntegrationTests.Setup;
using FluentAssertions;
using Xunit;

namespace AiTutor.IntegrationTests.Controllers;

public class UsersControllerTests : BaseIntegrationTest
{
    public UsersControllerTests(IntegrationTestWebAppFactory factory) : base(factory) { }

    [Fact]
    public async Task GetById_WithoutAuth_ShouldReturn401()
    {
        Client.DefaultRequestHeaders.Authorization = null;

        var response = await Client.GetAsync($"/api/Users/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetById_WithAuth_AndValidId_ShouldReturnUser()
    {
        var email = $"userget{Guid.NewGuid():N}@test.com";
        var token = await GetAuthTokenAsync(email);
        SetAuthHeader(token);

        // Obține user ID din token sau din register response
        var registerContent = CreateJsonContent(new
        {
            firstName = "Existing",
            lastName = "User",
            email = $"exists{Guid.NewGuid():N}@test.com",
            password = "Test123!@",
            role = 1
        });
        var regResponse = await Client.PostAsync("/api/Auth/register", registerContent);
        var regBody = await regResponse.Content.ReadAsStringAsync();
        var regData = JsonSerializer.Deserialize<JsonElement>(regBody, JsonOptions);
        var userId = regData.GetProperty("id").GetString();

        var response = await Client.GetAsync($"/api/Users/{userId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        var user = JsonSerializer.Deserialize<JsonElement>(body, JsonOptions);
        user.GetProperty("fullName").GetString().Should().Be("Existing User");
    }

    [Fact]
    public async Task GetById_WithInvalidId_ShouldReturn404()
    {
        var token = await GetAuthTokenAsync($"usr404{Guid.NewGuid():N}@test.com");
        SetAuthHeader(token);

        var response = await Client.GetAsync($"/api/Users/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
