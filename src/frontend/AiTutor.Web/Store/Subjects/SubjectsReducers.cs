using Fluxor;

namespace AiTutor.Web.Store.Subjects;

public static class SubjectsReducers
{
    [ReducerMethod]
    public static SubjectsState OnLoad(SubjectsState state, LoadSubjectsAction _)
        => state with { IsLoading = true, Error = null };

    [ReducerMethod]
    public static SubjectsState OnSuccess(SubjectsState state, LoadSubjectsSuccessAction action)
        => state with { IsLoading = false, Subjects = action.Subjects };

    [ReducerMethod]
    public static SubjectsState OnFailure(SubjectsState state, LoadSubjectsFailureAction action)
        => state with { IsLoading = false, Error = action.Error };

    [ReducerMethod]
    public static SubjectsState OnLessons(SubjectsState state, LoadLessonsSuccessAction action)
        => state with { Lessons = action.Lessons };
}
