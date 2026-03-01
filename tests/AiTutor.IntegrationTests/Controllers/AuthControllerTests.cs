using System.Net;
using System.Text.Json;
using AiTutor.IntegrationTests.Setup;
using FluentAssertions;

namespace AiTutor.IntegrationTests.Controllers;

public class AuthControllerTests : BaseIntegrationTest
{
    public AuthControllerTests(IntegrationTestWebAppFactory factory) : base(factory) { }

    [Fact]
    public async Task Register_WithValidData_ShouldReturn201()
    {
        // Arrange
        var content = CreateJsonContent(new
        {
            firstName = "John",
            lastName = "Doe",
            email = "john.doe@example.com",
            password = "Password123!",
            role = 1
        });

        // Act
        var response = await Client.PostAsync("/api/Auth/register", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Register_WithInvalidEmail_ShouldReturn400()
    {
        // Arrange
        var content = CreateJsonContent(new
        {
            firstName = "John",
            lastName = "Doe",
            email = "notanemail",
            password = "Password123!",
            role = 1
        });

        // Act
        var response = await Client.PostAsync("/api/Auth/register", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Login_WithValidCredentials_ShouldReturnToken()
    {
        // Arrange
        var email = "login.test@example.com";
        var password = "Password123!";

        var registerContent = CreateJsonContent(new
        {
            firstName = "Login",
            lastName = "Test",
            email,
            password,
            role = 1
        });
        await Client.PostAsync("/api/Auth/register", registerContent);

        var loginContent = CreateJsonContent(new { email, password });

        // Act
        var response = await Client.PostAsync("/api/Auth/login", loginContent);
        var body = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<JsonElement>(body, JsonOptions);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        data.GetProperty("token").GetString().Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Login_WithWrongPassword_ShouldReturn401()
    {
        // Arrange
        var content = CreateJsonContent(new
        {
            email = "wrong@example.com",
            password = "WrongPassword123!"
        });

        // Act
        var response = await Client.PostAsync("/api/Auth/login", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
