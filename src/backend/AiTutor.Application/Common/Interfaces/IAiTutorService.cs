using AiTutor.Domain.Enums;

namespace AiTutor.Application.Common.Interfaces;

public record ExplanationRequest(
    string Topic,
    SubjectType Subject,
    DifficultyLevel DifficultyLevel,
    int StudentAge
);

public record ExplanationResponse(
    string Topic,
    string Explanation,
    List<string> Examples,
    List<string> KeyPoints,
    SubjectType Subject,
    DifficultyLevel DifficultyLevel
);

public record HintRequest(
    string Question,
    SubjectType Subject
);

public record HintResponse(
    string Question,
    string Hint,
    SubjectType Subject
);

public record ProblemRequest(
    string Topic,
    SubjectType Subject,
    DifficultyLevel DifficultyLevel,
    int StudentAge,
    int Count
);

public record ProblemResponse(
    string Topic,
    List<string> Problems,
    List<string> Hints,
    List<string> Solutions,
    SubjectType Subject,
    DifficultyLevel DifficultyLevel
);

public interface IAiTutorService
{
    Task<ExplanationResponse> GetExplanationAsync(ExplanationRequest request, CancellationToken ct = default);
    Task<HintResponse> GetHintAsync(HintRequest request, CancellationToken ct = default);
    Task<ProblemResponse> GenerateProblemsAsync(ProblemRequest request, CancellationToken ct = default);
}
