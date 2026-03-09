using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using MediatR;

namespace AiTutor.Application.Features.AI.Queries.GenerateProblems;

public class GenerateProblemsQueryHandler : IRequestHandler<GenerateProblemsQuery, Result<ProblemResponse>>
{
    private readonly IAiTutorService _aiService;

    public GenerateProblemsQueryHandler(IAiTutorService aiService)
        => _aiService = aiService;

    public async Task<Result<ProblemResponse>> Handle(GenerateProblemsQuery request, CancellationToken ct)
    {
        var response = await _aiService.GenerateProblemsAsync(new ProblemRequest(
            request.Topic,
            request.Subject,
            request.DifficultyLevel,
            request.StudentAge,
            request.Count
        ), ct);

        return Result<ProblemResponse>.Success(response);
    }
}
