using Fluxor;

namespace AiTutor.Web.Store.Progress;

public static class ProgressReducers
{
    [ReducerMethod]
    public static ProgressState OnLoad(ProgressState state, LoadProgressAction _)
        => state with { IsLoading = true, Error = null };

    [ReducerMethod]
    public static ProgressState OnSuccess(ProgressState state, LoadProgressSuccessAction action)
        => state with { IsLoading = false, Items = action.Items };

    [ReducerMethod]
    public static ProgressState OnFailure(ProgressState state, LoadProgressFailureAction action)
        => state with { IsLoading = false, Error = action.Error };
}
