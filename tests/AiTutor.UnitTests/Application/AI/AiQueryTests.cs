using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Features.AI.Queries.GenerateProblems;
using AiTutor.Application.Features.AI.Queries.GetExplanation;
using AiTutor.Application.Features.AI.Queries.GetHint;
using AiTutor.Domain.Enums;
using FluentAssertions;
using Moq;
using Xunit;

namespace AiTutor.UnitTests.Application.AI;

public class AiQueryTests
{
    private readonly Mock<IAiTutorService> _aiServiceMock;

    public AiQueryTests()
    {
        _aiServiceMock = new Mock<IAiTutorService>();
    }

    // ── GenerateProblems ───────────────────────────────────────────────
    [Fact]
    public async Task GenerateProblems_ShouldReturnProblems()
    {
        var expectedResponse = new ProblemResponse(
            "Algebra", 
            new List<string> { "Problem 1", "Problem 2" },
            new List<string> { "Hint 1", "Hint 2" },
            new List<string> { "Solution 1", "Solution 2" },
            SubjectType.Mathematics, 
            DifficultyLevel.Beginner);

        _aiServiceMock.Setup(s => s.GenerateProblemsAsync(It.IsAny<ProblemRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        var handler = new GenerateProblemsQueryHandler(_aiServiceMock.Object);
        var query = new GenerateProblemsQuery("Algebra", SubjectType.Mathematics, DifficultyLevel.Beginner, 14, 2);

        var result = await handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Topic.Should().Be("Algebra");
        result.Data.Problems.Count.Should().Be(2);
        _aiServiceMock.Verify(s => s.GenerateProblemsAsync(
            It.Is<ProblemRequest>(r => r.Topic == "Algebra" && r.Count == 2),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    // ── GetExplanation ─────────────────────────────────────────────────
    [Fact]
    public async Task GetExplanation_ShouldReturnExplanation()
    {
        var expectedResponse = new ExplanationResponse(
            "Fractions",
            "Fractions are parts of a whole.",
            new List<string> { "1/2", "3/4" },
            new List<string> { "Numerator", "Denominator" },
            SubjectType.Mathematics,
            DifficultyLevel.Beginner);

        _aiServiceMock.Setup(s => s.GetExplanationAsync(It.IsAny<ExplanationRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        var handler = new GetExplanationQueryHandler(_aiServiceMock.Object);
        var query = new GetExplanationQuery("Fractions", SubjectType.Mathematics, DifficultyLevel.Beginner, 12);

        var result = await handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Topic.Should().Be("Fractions");
        result.Data.Explanation.Should().Contain("Fractions");
        _aiServiceMock.Verify(s => s.GetExplanationAsync(
            It.Is<ExplanationRequest>(r => r.Topic == "Fractions" && r.StudentAge == 12),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    // ── GetHint ────────────────────────────────────────────────────────
    [Fact]
    public async Task GetHint_ShouldReturnHint()
    {
        var expectedResponse = new HintResponse(
            "What is 2+2?",
            "Try counting on your fingers.",
            SubjectType.Mathematics);

        _aiServiceMock.Setup(s => s.GetHintAsync(It.IsAny<HintRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        var handler = new GetHintQueryHandler(_aiServiceMock.Object);
        var query = new GetHintQuery("What is 2+2?", SubjectType.Mathematics);

        var result = await handler.Handle(query, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Data!.Question.Should().Be("What is 2+2?");
        result.Data.Hint.Should().Be("Try counting on your fingers.");
        _aiServiceMock.Verify(s => s.GetHintAsync(
            It.Is<HintRequest>(r => r.Question == "What is 2+2?"),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
