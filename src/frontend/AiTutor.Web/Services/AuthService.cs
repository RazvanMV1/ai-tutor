using System.Net.Http.Headers;
using System.Text.Json;
using AiTutor.Web.Models;
using Microsoft.JSInterop;

namespace AiTutor.Web.Services;

public class AuthService
{
    private const string TokenKey = "auth_token";
    private const string UserKey = "auth_user";

    private readonly IJSRuntime _js;
    private readonly IHttpClientFactory _httpFactory;

    private string? _cachedToken;
    private AuthResponse? _cachedUser;

    public AuthService(IJSRuntime js, IHttpClientFactory httpFactory)
    {
        _js = js;
        _httpFactory = httpFactory;
    }

    public async Task SaveAuthAsync(AuthResponse response)
    {
        _cachedToken = response.Token;
        _cachedUser = response;

        await _js.InvokeVoidAsync("localStorage.setItem",
            TokenKey, response.Token);
        await _js.InvokeVoidAsync("localStorage.setItem",
            UserKey, JsonSerializer.Serialize(response));
    }

    public async Task<string?> GetTokenAsync()
    {
        if (_cachedToken is not null) return _cachedToken;
        _cachedToken = await _js.InvokeAsync<string?>(
            "localStorage.getItem", TokenKey);
        return _cachedToken;
    }

    public async Task<AuthResponse?> GetUserAsync()
    {
        if (_cachedUser is not null) return _cachedUser;

        var json = await _js.InvokeAsync<string?>(
            "localStorage.getItem", UserKey);
        if (json is null) return null;

        _cachedUser = JsonSerializer.Deserialize<AuthResponse>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return _cachedUser;
    }

    public async Task<bool> IsAuthenticatedAsync()
        => (await GetTokenAsync()) is not null;

    public async Task LogoutAsync()
    {
        _cachedToken = null;
        _cachedUser = null;
        await _js.InvokeVoidAsync("localStorage.removeItem", TokenKey);
        await _js.InvokeVoidAsync("localStorage.removeItem", UserKey);
    }

    public HttpClient GetUnauthenticatedClient()
        => _httpFactory.CreateClient("BackendApi");

    public async Task<HttpClient> GetAuthenticatedClientAsync()
    {
        var client = _httpFactory.CreateClient("BackendApi");
        var token = await GetTokenAsync();

        if (!string.IsNullOrEmpty(token))
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

        return client;
    }

    // Helper — rol ca int pentru compatibilitate cu UI
    public async Task<int> GetRoleIntAsync()
    {
        var user = await GetUserAsync();
        return user?.Role switch
        {
            "Admin" => 4,
            "Teacher" => 3,
            "Parent" => 2,
            "Student" => 1,
            _ => 1
        };
    }
}
