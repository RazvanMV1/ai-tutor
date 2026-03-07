namespace AiTutor.Web.Models;

public record ExplanationRequest(
    string Topic,
    int Subject,
    int DifficultyLevel,
    int StudentAge);

public record ExplanationResponse(
    string Topic,
    string Explanation,
    List<string> Examples,
    List<string> KeyPoints,
    int Subject,
    int DifficultyLevel);

public record HintRequest(string Question, int Subject);

public record HintResponse(string Question, string Hint, int Subject);

public record ProblemRequest(
    string Topic,
    int Subject,
    int DifficultyLevel,
    int StudentAge,
    int Count);

public record ProblemResponse(
    string Topic,
    List<string> Problems,
    List<string> Hints,
    List<string> Solutions,
    int Subject,
    int DifficultyLevel);
