using AiTutor.Web.Models;
using Fluxor;

namespace AiTutor.Web.Store.Progress;

[FeatureState]
public record ProgressState
{
    public List<StudentProgressDto> Items { get; init; } = [];
    public bool IsLoading { get; init; }
    public string? Error { get; init; }
}
