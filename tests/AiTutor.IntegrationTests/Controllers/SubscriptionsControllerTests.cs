using System.Net;
using System.Text.Json;
using AiTutor.IntegrationTests.Setup;
using FluentAssertions;
using Xunit;

namespace AiTutor.IntegrationTests.Controllers;

public class SubscriptionsControllerTests : BaseIntegrationTest
{
    public SubscriptionsControllerTests(IntegrationTestWebAppFactory factory) : base(factory) { }

    private async Task<string> RegisterAndGetUserIdAsync(string email)
    {
        var content = CreateJsonContent(new
        {
            firstName = "Sub",
            lastName = "User",
            email,
            password = "Test123!@",
            role = 1
        });
        var response = await Client.PostAsync("/api/Auth/register", content);
        var body = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<JsonElement>(body, JsonOptions);
        return data.GetProperty("id").GetString()!;
    }

    [Fact]
    public async Task Create_WithValidData_ShouldReturn201()
    {
        var email = $"sub{Guid.NewGuid():N}@test.com";
        var token = await GetAuthTokenAsync(email);
        SetAuthHeader(token);
        var userId = await RegisterAndGetUserIdAsync($"subcreate{Guid.NewGuid():N}@test.com");

        var content = CreateJsonContent(new
        {
            userId,
            type = 2,
            price = 19.99,
            startDate = DateTime.UtcNow,
            endDate = DateTime.UtcNow.AddMonths(1)
        });

        var response = await Client.PostAsync("/api/Subscriptions", content);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task GetByUser_WithAuth_ShouldReturnSubscriptions()
    {
        var email = $"subget{Guid.NewGuid():N}@test.com";
        var token = await GetAuthTokenAsync(email);
        SetAuthHeader(token);
        var userId = await RegisterAndGetUserIdAsync($"subgetuser{Guid.NewGuid():N}@test.com");

        // Creează subscription
        var createContent = CreateJsonContent(new
        {
            userId,
            type = 2,
            price = 9.99,
            startDate = DateTime.UtcNow,
            endDate = DateTime.UtcNow.AddMonths(1)
        });
        await Client.PostAsync("/api/Subscriptions", createContent);

        var response = await Client.GetAsync($"/api/Subscriptions/user/{userId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        var subs = JsonSerializer.Deserialize<JsonElement>(body, JsonOptions);
        subs.GetArrayLength().Should().BeGreaterOrEqualTo(1);
    }

    [Fact]
    public async Task Cancel_WithValidData_ShouldReturnOk()
    {
        var email = $"subcancel{Guid.NewGuid():N}@test.com";
        var token = await GetAuthTokenAsync(email);
        SetAuthHeader(token);
        var userId = await RegisterAndGetUserIdAsync($"subcanceluser{Guid.NewGuid():N}@test.com");

        var createContent = CreateJsonContent(new
        {
            userId,
            type = 2,
            price = 9.99,
            startDate = DateTime.UtcNow,
            endDate = DateTime.UtcNow.AddMonths(1)
        });
        var createResponse = await Client.PostAsync("/api/Subscriptions", createContent);
        var createBody = await createResponse.Content.ReadAsStringAsync();
        var created = JsonSerializer.Deserialize<JsonElement>(createBody, JsonOptions);
        var subId = created.GetProperty("id").GetString();

        var response = await Client.DeleteAsync($"/api/Subscriptions/{subId}?userId={userId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAll_WithoutAuth_ShouldReturn401()
    {
        Client.DefaultRequestHeaders.Authorization = null;

        var response = await Client.GetAsync("/api/Subscriptions");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
