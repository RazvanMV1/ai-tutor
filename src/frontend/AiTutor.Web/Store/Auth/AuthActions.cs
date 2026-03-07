using AiTutor.Web.Models;

namespace AiTutor.Web.Store.Auth;

public record LoginAction(string Email, string Password);
public record LoginSuccessAction(AuthResponse Response);
public record LoginFailureAction(string Error);
public record LogoutAction;
public record SetAuthFromStorageAction(AuthResponse Response);
