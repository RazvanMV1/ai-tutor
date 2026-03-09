using Blazored.LocalStorage;
using AiTutor.Web.Models;
using System.Text.Json;

namespace AiTutor.Web.Services;

public class AuthService
{
    private readonly ILocalStorageService _localStorage;
    private const string TokenKey = "auth_token";
    private const string UserKey = "auth_user";

    public AuthService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task SaveAuthAsync(LoginResponse response)
    {
        await _localStorage.SetItemAsync(TokenKey, response.Token);
        await _localStorage.SetItemAsync(UserKey, response.User);
    }

    public async Task<string?> GetTokenAsync()
    {
        return await _localStorage.GetItemAsync<string>(TokenKey);
    }

    public async Task<UserDto?> GetCurrentUserAsync()
    {
        return await _localStorage.GetItemAsync<UserDto>(UserKey);
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await GetTokenAsync();
        if (string.IsNullOrEmpty(token)) return false;

        try
        {
            var expiry = GetTokenExpiry(token);
            return expiry > DateTime.UtcNow;
        }
        catch
        {
            return false;
        }
    }

    private static DateTime GetTokenExpiry(string token)
    {
        var parts = token.Split('.');
        if (parts.Length != 3) return DateTime.MinValue;

        var payload = parts[1];
        // Pad base64url string
        int mod4 = payload.Length % 4;
        string padded = mod4 switch
        {
            2 => payload + "==",
            3 => payload + "=",
            _ => payload
        };
        padded = padded.Replace('-', '+').Replace('_', '/');
        var jsonBytes = Convert.FromBase64String(padded);
        var json = System.Text.Encoding.UTF8.GetString(jsonBytes);
        using var doc = JsonDocument.Parse(json);
        if (doc.RootElement.TryGetProperty("exp", out var exp))
        {
            var epochSeconds = exp.GetInt64();
            return DateTimeOffset.FromUnixTimeSeconds(epochSeconds).UtcDateTime;
        }
        return DateTime.MaxValue;
    }

    public async Task<Guid> GetCurrentUserIdAsync()
    {
        var user = await GetCurrentUserAsync();
        return user?.Id ?? Guid.Empty;
    }

    public async Task<UserRole> GetCurrentUserRoleAsync()
    {
        var user = await GetCurrentUserAsync();
        return user?.Role ?? UserRole.Student;
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync(TokenKey);
        await _localStorage.RemoveItemAsync(UserKey);
    }
}
