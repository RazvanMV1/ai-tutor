using AiTutor.Web.Models;
using Fluxor;

namespace AiTutor.Web.Store.Subjects;

[FeatureState]
public record SubjectsState
{
    public List<SubjectDto> Subjects { get; init; } = [];
    public List<LessonDto> Lessons { get; init; } = [];
    public bool IsLoading { get; init; }
    public string? Error { get; init; }
}
