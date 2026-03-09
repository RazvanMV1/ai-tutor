using Fluxor;

namespace AiTutor.Web.Store.Auth;

public static class AuthReducers
{
    [ReducerMethod]
    public static AuthState OnLoginAction(AuthState state, LoginAction _)
        => state with { IsLoading = true, Error = null };

    [ReducerMethod]
    public static AuthState OnLoginSuccess(AuthState state, LoginSuccessAction action)
        => state with
        {
            IsLoading = false,
            Token = action.Response.Token,
            User = action.Response,
            Error = null
        };

    [ReducerMethod]
    public static AuthState OnLoginFailure(AuthState state, LoginFailureAction action)
        => state with { IsLoading = false, Error = action.Error };

    [ReducerMethod]
    public static AuthState OnLogout(AuthState state, LogoutAction _)
        => state with { Token = null, User = null, Error = null };

    [ReducerMethod]
    public static AuthState OnSetFromStorage(AuthState state,
        SetAuthFromStorageAction action)
        => state with { Token = action.Response.Token, User = action.Response };
}
