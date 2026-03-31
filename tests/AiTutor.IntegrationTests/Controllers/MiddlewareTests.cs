using System.Net;
using AiTutor.IntegrationTests.Setup;
using FluentAssertions;
using Xunit;

namespace AiTutor.IntegrationTests.Controllers;

public class MiddlewareTests : BaseIntegrationTest
{
    public MiddlewareTests(IntegrationTestWebAppFactory factory) : base(factory) { }

    [Fact]
    public async Task NotFoundEntity_ShouldReturn404WithJsonBody()
    {
        var token = await GetAuthTokenAsync($"mid{Guid.NewGuid():N}@test.com");
        SetAuthHeader(token);

        var response = await Client.GetAsync($"/api/Users/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Content.Headers.ContentType!.MediaType.Should().Be("application/json");
    }

    [Fact]
    public async Task UnknownEndpoint_ShouldReturn404()
    {
        var token = await GetAuthTokenAsync($"mid2{Guid.NewGuid():N}@test.com");
        SetAuthHeader(token);

        var response = await Client.GetAsync("/api/NonExistent");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
