using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Application.Features.Subscriptions.Commands.CreateSubscription;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Subscriptions.Queries.GetAllSubscriptions;

public class GetAllSubscriptionsQueryHandler
    : IRequestHandler<GetAllSubscriptionsQuery, Result<PaginatedList<SubscriptionResponse>>>
{
    private readonly IApplicationDbContext _context;

    public GetAllSubscriptionsQueryHandler(IApplicationDbContext context)
        => _context = context;

    public async Task<Result<PaginatedList<SubscriptionResponse>>> Handle(
        GetAllSubscriptionsQuery request, CancellationToken ct)
    {
        var query = _context.Subscriptions
            .OrderByDescending(s => s.StartDate);

        var total = await query.CountAsync(ct);

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

        var response = items.Select(s => new SubscriptionResponse(
            s.Id,
            s.UserId,
            s.Type,
            s.Price,
            s.StartDate,
            s.EndDate,
            s.IsValid()
        )).ToList();

        return Result<PaginatedList<SubscriptionResponse>>.Success(
            new PaginatedList<SubscriptionResponse>(response, total, request.Page, request.PageSize));
    }
}
