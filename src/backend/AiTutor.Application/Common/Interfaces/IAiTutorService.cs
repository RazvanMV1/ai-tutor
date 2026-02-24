namespace AiTutor.Application.Common.Interfaces;

public interface IAiTutorService
{
    Task<string> GetExplanationAsync(string topic, string subject, int difficultyLevel, CancellationToken cancellationToken);
    Task<string> GenerateProblemAsync(string topic, string subject, int difficultyLevel, CancellationToken cancellationToken);
    Task<string> GetHintAsync(string question, string subject, CancellationToken cancellationToken);
}
