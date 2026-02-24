using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Domain.Entities;
using MediatR;

namespace AiTutor.Application.Features.Subjects.Commands.CreateSubject;

public class CreateSubjectCommandHandler : IRequestHandler<CreateSubjectCommand, Result<SubjectResponse>>
{
    private readonly IApplicationDbContext _context;

    public CreateSubjectCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<SubjectResponse>> Handle(CreateSubjectCommand request,
        CancellationToken cancellationToken)
    {
        var subject = Subject.Create(request.Name, request.Description, request.Type);

        _context.Subjects.Add(subject);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<SubjectResponse>.Success(
            new SubjectResponse(subject.Id, subject.Name, subject.Type), 201);
    }
}
