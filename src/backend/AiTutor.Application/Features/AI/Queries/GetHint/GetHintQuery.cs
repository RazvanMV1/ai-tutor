using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Common.Models;
using AiTutor.Domain.Enums;
using MediatR;

namespace AiTutor.Application.Features.AI.Queries.GetHint;

public record GetHintQuery(
    string Question,
    SubjectType Subject
) : IRequest<Result<HintResponse>>;
