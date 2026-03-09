using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Domain.Enums;
using MediatR;

namespace AiTutor.Application.Features.AI.Queries.GenerateProblems;

public record GenerateProblemsQuery(
    string Topic,
    SubjectType Subject,
    DifficultyLevel DifficultyLevel,
    int StudentAge,
    int Count = 3
) : IRequest<Result<ProblemResponse>>;
