using AiTutor.Application.Common.Exceptions;
using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Subscriptions.Commands.CreateSubscription;

public class CreateSubscriptionCommandHandler : IRequestHandler<CreateSubscriptionCommand, Result<SubscriptionResponse>>
{
    private readonly IApplicationDbContext _context;

    public CreateSubscriptionCommandHandler(IApplicationDbContext context)
        => _context = context;

    public async Task<Result<SubscriptionResponse>> Handle(
        CreateSubscriptionCommand request, CancellationToken ct)
    {
        var userExists = await _context.Users
            .AnyAsync(u => u.Id == request.UserId, ct);

        if (!userExists)
            throw new NotFoundException(nameof(User), request.UserId);

        var subscription = Subscription.Create(
            request.UserId,
            request.Type,
            request.StartDate,
            request.EndDate,
            request.Price
        );

        _context.Subscriptions.Add(subscription);
        await _context.SaveChangesAsync(ct);

        return Result<SubscriptionResponse>.Success(new SubscriptionResponse(
            subscription.Id,
            subscription.UserId,
            subscription.Type,
            subscription.Price,
            subscription.StartDate,
            subscription.EndDate,
            subscription.IsValid()
        ), 201);
    }
}
