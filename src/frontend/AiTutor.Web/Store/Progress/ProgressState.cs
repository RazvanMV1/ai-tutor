using AiTutor.Web.Models;
using Fluxor;

namespace AiTutor.Web.Store.Progress;

[FeatureState]
public record ProgressStateRecord
{
    public List<StudentProgressDto> ProgressItems { get; init; } = new();
    public bool IsLoading { get; init; } = false;
    public string? Error { get; init; }
}

// Actions
public record LoadProgressAction(Guid UserId);
public record LoadProgressSuccessAction(List<StudentProgressDto> Items);
public record LoadProgressFailureAction(string Error);

// Reducers
public static class ProgressReducers
{
    [ReducerMethod]
    public static ProgressStateRecord OnLoadProgress(ProgressStateRecord state, LoadProgressAction action) =>
        state with { IsLoading = true, Error = null };

    [ReducerMethod]
    public static ProgressStateRecord OnLoadProgressSuccess(ProgressStateRecord state, LoadProgressSuccessAction action) =>
        state with { IsLoading = false, ProgressItems = action.Items };

    [ReducerMethod]
    public static ProgressStateRecord OnLoadProgressFailure(ProgressStateRecord state, LoadProgressFailureAction action) =>
        state with { IsLoading = false, Error = action.Error };
}
