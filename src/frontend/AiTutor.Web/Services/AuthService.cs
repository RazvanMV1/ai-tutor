using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using AiTutor.Web.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AiTutor.Web.Services;

public class AuthService
{
    private readonly IJSRuntime _js;
    private readonly HttpClient _httpClient;
    private readonly NavigationManager _nav;

    private const string TokenKey = "auth_token";
    private const string UserKey = "auth_user";

    public AuthService(IJSRuntime js, IHttpClientFactory factory, NavigationManager nav)
    {
        _js = js;
        _httpClient = factory.CreateClient("BackendApi");
        _nav = nav;
    }

    // ── localStorage ─────────────────────────────────────────────────────────

    public async Task SaveTokenAsync(AuthResponse authResponse)
    {
        await _js.InvokeVoidAsync("localStorage.setItem", TokenKey, authResponse.Token);
        var userJson = JsonSerializer.Serialize(authResponse);
        await _js.InvokeVoidAsync("localStorage.setItem", UserKey, userJson);
    }

    public async Task<string?> GetTokenAsync()
        => await _js.InvokeAsync<string?>("localStorage.getItem", TokenKey);

    public async Task<AuthResponse?> GetCurrentUserAsync()
    {
        var json = await _js.InvokeAsync<string?>("localStorage.getItem", UserKey);
        if (string.IsNullOrEmpty(json)) return null;
        try { return JsonSerializer.Deserialize<AuthResponse>(json); }
        catch { return null; }
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await GetTokenAsync();
        if (string.IsNullOrEmpty(token)) return false;
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            return jwt.ValidTo > DateTime.UtcNow;
        }
        catch { return false; }
    }

    public async Task LogoutAsync()
    {
        await _js.InvokeVoidAsync("localStorage.removeItem", TokenKey);
        await _js.InvokeVoidAsync("localStorage.removeItem", UserKey);
        _nav.NavigateTo("/login");
    }

    // ── API Calls ────────────────────────────────────────────────────────────

    public async Task<(bool Success, string? Error, AuthResponse? Data)>
        LoginAsync(LoginRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/auth/login", request);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<AuthResponse>();
                return (true, null, data);
            }
            var error = await response.Content.ReadAsStringAsync();
            return (false, $"Eroare {(int)response.StatusCode}: {error}", null);
        }
        catch (Exception ex)
        {
            return (false, $"Conexiunea a eșuat: {ex.Message}", null);
        }
    }

    public async Task<(bool Success, string? Error)>
        RegisterAsync(RegisterRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/auth/register", request);
            if (response.IsSuccessStatusCode) return (true, null);
            var error = await response.Content.ReadAsStringAsync();
            return (false, $"Eroare {(int)response.StatusCode}: {error}");
        }
        catch (Exception ex)
        {
            return (false, $"Conexiunea a eșuat: {ex.Message}");
        }
    }
}
