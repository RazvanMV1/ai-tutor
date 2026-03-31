using AiTutor.Application.Common.Models;
using FluentAssertions;
using Xunit;

namespace AiTutor.UnitTests.Application.Common;

public class ResultTests
{
    [Fact]
    public void Success_ShouldSetPropertiesCorrectly()
    {
        var result = Result<string>.Success("hello");

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be("hello");
        result.Error.Should().BeNull();
        result.StatusCode.Should().Be(200);
    }

    [Fact]
    public void Success_WithCustomStatusCode_ShouldSetStatusCode()
    {
        var result = Result<string>.Success("created", 201);

        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(201);
    }

    [Fact]
    public void Failure_ShouldSetPropertiesCorrectly()
    {
        var result = Result<string>.Failure("something went wrong");

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("something went wrong");
        result.Data.Should().BeNull();
        result.StatusCode.Should().Be(400);
    }

    [Fact]
    public void Failure_WithCustomStatusCode_ShouldSetStatusCode()
    {
        var result = Result<string>.Failure("not found", 404);

        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(404);
    }
}
