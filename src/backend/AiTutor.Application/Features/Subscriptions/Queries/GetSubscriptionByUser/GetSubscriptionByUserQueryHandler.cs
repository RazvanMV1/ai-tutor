using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Application.Features.Subscriptions.Commands.CreateSubscription;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Subscriptions.Queries.GetSubscriptionByUser;

public class GetSubscriptionByUserQueryHandler
    : IRequestHandler<GetSubscriptionByUserQuery, Result<List<SubscriptionResponse>>>
{
    private readonly IApplicationDbContext _context;

    public GetSubscriptionByUserQueryHandler(IApplicationDbContext context)
        => _context = context;

    public async Task<Result<List<SubscriptionResponse>>> Handle(
        GetSubscriptionByUserQuery request, CancellationToken ct)
    {
        var subscriptions = await _context.Subscriptions
            .Where(s => s.UserId == request.UserId)
            .OrderByDescending(s => s.StartDate)
            .ToListAsync(ct);

        var response = subscriptions.Select(s => new SubscriptionResponse(
            s.Id,
            s.UserId,
            s.Type,
            s.Price,
            s.StartDate,
            s.EndDate,
            s.IsValid()
        )).ToList();

        return Result<List<SubscriptionResponse>>.Success(response);
    }
}
