using AiTutor.Web.Models;
using AiTutor.Web.Services;
using Fluxor;
using Microsoft.AspNetCore.Components;

namespace AiTutor.Web.Store.Auth;

public class AuthEffects
{
    private readonly ApiService _apiService;
    private readonly AuthService _authService;
    private readonly NavigationManager _navigation;

    public AuthEffects(ApiService apiService, AuthService authService, NavigationManager navigation)
    {
        _apiService = apiService;
        _authService = authService;
        _navigation = navigation;
    }

    [EffectMethod]
    public async Task HandleLogin(LoginAction action, IDispatcher dispatcher)
    {
        try
        {
            var response = await _apiService.LoginAsync(new LoginRequest
            {
                Email = action.Email,
                Password = action.Password
            });

            if (response != null)
            {
                await _authService.SaveAuthAsync(response);
                dispatcher.Dispatch(new LoginSuccessAction(response));
                _navigation.NavigateTo("/dashboard");
            }
            else
            {
                dispatcher.Dispatch(new LoginFailureAction("Email sau parolă incorectă."));
            }
        }
        catch (Exception ex)
        {
            dispatcher.Dispatch(new LoginFailureAction($"Eroare: {ex.Message}"));
        }
    }

    [EffectMethod]
    public async Task HandleLogout(LogoutAction action, IDispatcher dispatcher)
    {
        await _authService.LogoutAsync();
        _navigation.NavigateTo("/login");
    }
}
