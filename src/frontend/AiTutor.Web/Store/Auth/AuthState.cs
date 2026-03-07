using AiTutor.Web.Models;
using Fluxor;

namespace AiTutor.Web.Store.Auth;

[FeatureState]
public record AuthState
{
    public UserInfo? User { get; init; }
    public string? Token { get; init; }
    public bool IsLoading { get; init; }
    public string? Error { get; init; }

    public bool IsAuthenticated => Token is not null;
}
