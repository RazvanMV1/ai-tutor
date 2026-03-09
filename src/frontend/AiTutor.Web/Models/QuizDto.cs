namespace AiTutor.Web.Models;

public record QuizDto(
    Guid Id,
    string Title,
    Guid LessonId,
    List<QuestionDto> Questions);

public record QuestionDto(
    Guid Id,
    string Text,
    List<string> Options,
    int CorrectOptionIndex);

public record CreateQuizRequest(
    string Title,
    Guid LessonId);

public record AddQuestionRequest(
    string Text,
    List<string> Options,
    int CorrectOptionIndex);

public record SubmitQuizRequest(
    Guid UserId,
    List<QuizAnswer> Answers);

public record QuizAnswer(
    Guid Id,
    int SelectedOptionIndex);

// ⚠️ Quiz result are structură detaliată cu results[]
public record QuizResultDto(
    Guid QuizId,
    Guid UserId,
    int TotalQuestions,
    int CorrectAnswers,
    double ScorePercentage,
    List<QuizResultItem> Results);

public record QuizResultItem(
    Guid QuestionId,
    string QuestionText,
    string UserAnswer,
    string CorrectAnswer,
    bool IsCorrect,
    string Explanation);
