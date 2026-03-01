using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Domain.Entities;
using MediatR;

namespace AiTutor.Application.Features.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<CreateUserResponse>>
{
    private readonly IApplicationDbContext _context;

    public CreateUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<CreateUserResponse>> Handle(CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = User.Create(request.FirstName, request.LastName, request.Email, request.Role);

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<CreateUserResponse>.Success(
            new CreateUserResponse(user.Id, user.FullName, user.Email.Value, user.Role), 201);
    }
}
