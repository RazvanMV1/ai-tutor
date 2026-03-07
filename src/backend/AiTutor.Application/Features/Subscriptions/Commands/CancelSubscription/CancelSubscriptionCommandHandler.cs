using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Subscriptions.Commands.CancelSubscription;

public class CancelSubscriptionCommandHandler : IRequestHandler<CancelSubscriptionCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;

    public CancelSubscriptionCommandHandler(IApplicationDbContext context)
        => _context = context;

    public async Task<Result<bool>> Handle(CancelSubscriptionCommand request, CancellationToken ct)
    {
        var subscription = await _context.Subscriptions
            .FirstOrDefaultAsync(s => s.Id == request.SubscriptionId, ct);

        if (subscription is null)
            throw new NotFoundException(nameof(Subscription), request.SubscriptionId);

        if (subscription.UserId != request.UserId)
            throw new ForbiddenAccessException();

        subscription.Deactivate();
        await _context.SaveChangesAsync(ct);

        return Result<bool>.Success(true);
    }
}
