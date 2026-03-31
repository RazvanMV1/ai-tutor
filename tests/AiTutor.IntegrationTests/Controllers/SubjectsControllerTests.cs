using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using AiTutor.IntegrationTests.Setup;
using FluentAssertions;
using Xunit;

namespace AiTutor.IntegrationTests.Controllers;

public class SubjectsControllerTests : BaseIntegrationTest
{
    public SubjectsControllerTests(IntegrationTestWebAppFactory factory) : base(factory) { }

    [Fact]
    public async Task GetAll_WithoutAuth_ShouldReturn401()
    {
        Client.DefaultRequestHeaders.Authorization = null;

        var response = await Client.GetAsync("/api/Subjects");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetAll_WithAuth_ShouldReturnSeededSubjects()
    {
        var token = await GetAuthTokenAsync($"sub{Guid.NewGuid():N}@test.com");
        SetAuthHeader(token);

        var response = await Client.GetAsync("/api/Subjects");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        var subjects = JsonSerializer.Deserialize<JsonElement>(body, JsonOptions);
        subjects.GetArrayLength().Should().BeGreaterOrEqualTo(3);
    }
}
