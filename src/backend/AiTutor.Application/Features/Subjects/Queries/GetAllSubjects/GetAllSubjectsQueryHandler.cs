using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.Application.Features.Subjects.Queries.GetAllSubjects;

public class GetAllSubjectsQueryHandler : IRequestHandler<GetAllSubjectsQuery, Result<List<SubjectDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetAllSubjectsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<SubjectDto>>> Handle(GetAllSubjectsQuery request,
        CancellationToken cancellationToken)
    {
        var subjects = await _context.Subjects
            .AsNoTracking()
            .Select(s => new SubjectDto(s.Id, s.Name, s.Description, s.Type))
            .ToListAsync(cancellationToken);

        return Result<List<SubjectDto>>.Success(subjects);
    }
}
