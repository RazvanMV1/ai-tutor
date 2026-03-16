using AiTutor.Web.Services;
using Fluxor;

namespace AiTutor.Web.Store.Subjects;

public class SubjectsEffects
{
    private readonly ApiService _apiService;

    public SubjectsEffects(ApiService apiService)
    {
        _apiService = apiService;
    }

    [EffectMethod]
    public async Task HandleLoadSubjects(LoadSubjectsAction action, IDispatcher dispatcher)
    {
        try
        {
            var subjects = await _apiService.GetSubjectsAsync();
            dispatcher.Dispatch(new LoadSubjectsSuccessAction(subjects));
        }
        catch (Exception ex)
        {
            dispatcher.Dispatch(new LoadSubjectsFailureAction(ex.Message));
        }
    }
}
