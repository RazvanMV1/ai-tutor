using AiTutor.Application.Common.Behaviors;
using AiTutor.Application.Common.Models;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AiTutor.UnitTests.Application.Common;

public class LoggingBehaviorTests
{
    public record TestCommand(string Name) : IRequest<Result<string>>;

    [Fact]
    public async Task LoggingBehavior_ShouldLogAndCallNext()
    {
        var loggerMock = new Mock<ILogger<LoggingBehavior<TestCommand, Result<string>>>>();
        var behavior = new LoggingBehavior<TestCommand, Result<string>>(loggerMock.Object);
        var expectedResult = Result<string>.Success("ok");

        var result = await behavior.Handle(
            new TestCommand("test"),
            () => Task.FromResult(expectedResult),
            CancellationToken.None);

        result.Should().Be(expectedResult);
        loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Exactly(2));
    }
}
