using AiTutor.Domain.Common;
using AiTutor.Domain.Exceptions;

namespace AiTutor.Domain.Entities;

public class Question : BaseEntity
{
    public string Text { get; private set; } = string.Empty;
    public string CorrectAnswer { get; private set; } = string.Empty;
    public List<string> Options { get; private set; } = new();
    public string? Explanation { get; private set; }
    public int Points { get; private set; }
    public Guid QuizId { get; private set; }
    public Quiz Quiz { get; private set; } = null!;

    private Question() { }

    public static Question Create(string text, string correctAnswer,
        List<string> options, int points, Guid quizId, string? explanation = null)
    {
        if (!options.Contains(correctAnswer))
            throw new DomainException("Correct answer must be one of the options.");

        if (options.Count < 2)
            throw new DomainException("Question must have at least 2 options.");

        return new Question
        {
            Text = text,
            CorrectAnswer = correctAnswer,
            Options = options,
            Points = points,
            QuizId = quizId,
            Explanation = explanation
        };
    }

    public bool IsCorrect(string answer) =>
        CorrectAnswer.Equals(answer, StringComparison.OrdinalIgnoreCase);
}
