using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using MediatR;

namespace AiTutor.Application.Features.AI.Queries.GetExplanation;

public class GetExplanationQueryHandler : IRequestHandler<GetExplanationQuery, Result<ExplanationResponse>>
{
    private readonly IAiTutorService _aiService;

    public GetExplanationQueryHandler(IAiTutorService aiService)
        => _aiService = aiService;

    public async Task<Result<ExplanationResponse>> Handle(GetExplanationQuery request, CancellationToken ct)
    {
        var response = await _aiService.GetExplanationAsync(new ExplanationRequest(
            request.Topic,
            request.Subject,
            request.DifficultyLevel,
            request.StudentAge
        ), ct);

        return Result<ExplanationResponse>.Success(response);
    }
}
