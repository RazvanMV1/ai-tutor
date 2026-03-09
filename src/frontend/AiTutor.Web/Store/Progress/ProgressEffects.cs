using AiTutor.Web.Services;
using Fluxor;

namespace AiTutor.Web.Store.Progress;

public class ProgressEffects
{
    private readonly ApiService _apiService;

    public ProgressEffects(ApiService apiService)
    {
        _apiService = apiService;
    }

    [EffectMethod]
    public async Task HandleLoadProgress(LoadProgressAction action, IDispatcher dispatcher)
    {
        try
        {
            var items = await _apiService.GetProgressAsync(action.UserId);
            dispatcher.Dispatch(new LoadProgressSuccessAction(items));
        }
        catch (Exception ex)
        {
            dispatcher.Dispatch(new LoadProgressFailureAction(ex.Message));
        }
    }
}
