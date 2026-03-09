using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using MediatR;

namespace AiTutor.Application.Features.AI.Queries.GetHint;

public class GetHintQueryHandler : IRequestHandler<GetHintQuery, Result<HintResponse>>
{
    private readonly IAiTutorService _aiService;

    public GetHintQueryHandler(IAiTutorService aiService)
        => _aiService = aiService;

    public async Task<Result<HintResponse>> Handle(GetHintQuery request, CancellationToken ct)
    {
        var response = await _aiService.GetHintAsync(new HintRequest(
            request.Question,
            request.Subject
        ), ct);

        return Result<HintResponse>.Success(response);
    }
}
