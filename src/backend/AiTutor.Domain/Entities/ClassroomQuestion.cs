using AiTutor.Domain.Common;
using AiTutor.Domain.Exceptions;

namespace AiTutor.Domain.Entities;

public class ClassroomQuestion : BaseEntity
{
    public Guid ClassroomQuizId { get; private set; }
    public ClassroomQuiz Quiz { get; private set; } = null!;
    public string Text { get; private set; } = string.Empty;
    public string CorrectAnswer { get; private set; } = string.Empty;
    public List<string> Options { get; private set; } = new();
    public int Points { get; private set; }
    public string? Explanation { get; private set; }

    private ClassroomQuestion() { }

    public static ClassroomQuestion Create(Guid quizId, string text, string correctAnswer,
        List<string> options, int points, string? explanation = null)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new DomainException("Question text cannot be empty.");
        if (!options.Contains(correctAnswer))
            throw new DomainException("Correct answer must be one of the options.");

        return new ClassroomQuestion
        {
            ClassroomQuizId = quizId,
            Text = text,
            CorrectAnswer = correctAnswer,
            Options = options,
            Points = points,
            Explanation = explanation
        };
    }

    public bool IsCorrect(string answer) =>
        string.Equals(CorrectAnswer, answer, StringComparison.OrdinalIgnoreCase);
}
