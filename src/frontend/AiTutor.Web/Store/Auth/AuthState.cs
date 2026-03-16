using AiTutor.Web.Models;
using Fluxor;

namespace AiTutor.Web.Store.Auth;

[FeatureState]
public record AuthStateRecord
{
    public bool IsAuthenticated { get; init; } = false;
    public bool IsLoading { get; init; } = false;
    public string? Token { get; init; }
    public UserDto? User { get; init; }
    public string? Error { get; init; }
}

// Actions
public record LoginAction(string Email, string Password);
public record LoginSuccessAction(LoginResponse Response);
public record LoginFailureAction(string Error);
public record LogoutAction;
public record SetAuthFromStorageAction(string Token, UserDto User);

// Reducers
public static class AuthReducers
{
    [ReducerMethod]
    public static AuthStateRecord OnLogin(AuthStateRecord state, LoginAction action) =>
        state with { IsLoading = true, Error = null };

    [ReducerMethod]
    public static AuthStateRecord OnLoginSuccess(AuthStateRecord state, LoginSuccessAction action) =>
        state with
        {
            IsLoading = false,
            IsAuthenticated = true,
            Token = action.Response.Token,
            User = action.Response.User,
            Error = null
        };

    [ReducerMethod]
    public static AuthStateRecord OnLoginFailure(AuthStateRecord state, LoginFailureAction action) =>
        state with { IsLoading = false, Error = action.Error, IsAuthenticated = false };

    [ReducerMethod]
    public static AuthStateRecord OnLogout(AuthStateRecord state, LogoutAction action) =>
        new AuthStateRecord();

    [ReducerMethod]
    public static AuthStateRecord OnSetAuthFromStorage(AuthStateRecord state, SetAuthFromStorageAction action) =>
        state with
        {
            IsAuthenticated = true,
            Token = action.Token,
            User = action.User
        };
}
