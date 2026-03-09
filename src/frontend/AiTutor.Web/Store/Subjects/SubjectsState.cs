using AiTutor.Web.Models;
using Fluxor;

namespace AiTutor.Web.Store.Subjects;

[FeatureState]
public record SubjectsStateRecord
{
    public List<SubjectDto> Subjects { get; init; } = new();
    public bool IsLoading { get; init; } = false;
    public string? Error { get; init; }
}

// Actions
public record LoadSubjectsAction;
public record LoadSubjectsSuccessAction(List<SubjectDto> Subjects);
public record LoadSubjectsFailureAction(string Error);

// Reducers
public static class SubjectsReducers
{
    [ReducerMethod]
    public static SubjectsStateRecord OnLoadSubjects(SubjectsStateRecord state, LoadSubjectsAction action) =>
        state with { IsLoading = true, Error = null };

    [ReducerMethod]
    public static SubjectsStateRecord OnLoadSubjectsSuccess(SubjectsStateRecord state, LoadSubjectsSuccessAction action) =>
        state with { IsLoading = false, Subjects = action.Subjects };

    [ReducerMethod]
    public static SubjectsStateRecord OnLoadSubjectsFailure(SubjectsStateRecord state, LoadSubjectsFailureAction action) =>
        state with { IsLoading = false, Error = action.Error };
}
