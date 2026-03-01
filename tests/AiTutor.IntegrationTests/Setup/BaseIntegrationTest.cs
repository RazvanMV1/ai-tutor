using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AiTutor.IntegrationTests.Setup;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
{
    protected readonly HttpClient Client;
    protected readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        Client = factory.CreateClient();
    }

    protected StringContent CreateJsonContent(object obj)
    {
        var json = JsonSerializer.Serialize(obj);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    protected async Task<string> GetAuthTokenAsync(
    string email = "test@example.com",
    string password = "Test123!@",
    int role = 1)
    {
        // Register
        var registerContent = CreateJsonContent(new
        {
            firstName = "Test",
            lastName = "User",
            email,
            password,
            role
        });
        await Client.PostAsync("/api/Auth/register", registerContent);

        // Login
        var loginContent = CreateJsonContent(new { email, password });
        var loginResponse = await Client.PostAsync("/api/Auth/login", loginContent);
        var loginBody = await loginResponse.Content.ReadAsStringAsync();
        var loginData = JsonSerializer.Deserialize<JsonElement>(loginBody, JsonOptions);

        return loginData.GetProperty("token").GetString()!;
    }


    protected void SetAuthHeader(string token)
    {
        Client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }
}
