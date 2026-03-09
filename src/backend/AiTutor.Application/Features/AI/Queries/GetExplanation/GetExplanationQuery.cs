using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Domain.Enums;
using MediatR;

namespace AiTutor.Application.Features.AI.Queries.GetExplanation;

public record GetExplanationQuery(
    string Topic,
    SubjectType Subject,
    DifficultyLevel DifficultyLevel,
    int StudentAge
) : IRequest<Result<ExplanationResponse>>;
