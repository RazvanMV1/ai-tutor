using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using AiTutor.IntegrationTests.Setup;
using FluentAssertions;
using Xunit;

namespace AiTutor.IntegrationTests.Controllers;

public class AuthControllerTests : BaseIntegrationTest
{
    public AuthControllerTests(IntegrationTestWebAppFactory factory) : base(factory) { }

    [Fact]
    public async Task Register_WithValidData_ShouldReturn201()
    {
        var content = CreateJsonContent(new
        {
            firstName = "John",
            lastName = "Doe",
            email = $"john{Guid.NewGuid():N}@test.com",
            password = "Test123!@",
            role = 1
        });

        var response = await Client.PostAsync("/api/Auth/register", content);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var body = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<JsonElement>(body, JsonOptions);
        data.GetProperty("id").GetString().Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Register_WithDuplicateEmail_ShouldReturnBadRequest()
    {
        var email = $"dup{Guid.NewGuid():N}@test.com";
        var content = CreateJsonContent(new
        {
            firstName = "Test",
            lastName = "User",
            email,
            password = "Test123!@",
            role = 1
        });

        await Client.PostAsync("/api/Auth/register", content);
        var response = await Client.PostAsync("/api/Auth/register", content);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Login_WithValidCredentials_ShouldReturnToken()
    {
        var email = $"login{Guid.NewGuid():N}@test.com";
        var password = "Test123!@";

        var registerContent = CreateJsonContent(new
        {
            firstName = "Test",
            lastName = "User",
            email,
            password,
            role = 1
        });
        await Client.PostAsync("/api/Auth/register", registerContent);

        var loginContent = CreateJsonContent(new { email, password });
        var response = await Client.PostAsync("/api/Auth/login", loginContent);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<JsonElement>(body, JsonOptions);
        data.GetProperty("token").GetString().Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ShouldReturn401()
    {
        var content = CreateJsonContent(new
        {
            email = "nonexistent@test.com",
            password = "WrongPass1!@"
        });

        var response = await Client.PostAsync("/api/Auth/login", content);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Login_WithWrongPassword_ShouldReturn401()
    {
        var email = $"wrongpw{Guid.NewGuid():N}@test.com";
        var registerContent = CreateJsonContent(new
        {
            firstName = "Test",
            lastName = "User",
            email,
            password = "Test123!@",
            role = 1
        });
        await Client.PostAsync("/api/Auth/register", registerContent);

        var loginContent = CreateJsonContent(new { email, password = "WrongPass1!@" });
        var response = await Client.PostAsync("/api/Auth/login", loginContent);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
