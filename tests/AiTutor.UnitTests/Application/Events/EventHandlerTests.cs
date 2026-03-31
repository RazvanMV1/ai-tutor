using AiTutor.Application.Features.Progress.Events;
using AiTutor.Application.Features.Users.Events;
using AiTutor.Domain.Events;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AiTutor.UnitTests.Application.Events;

public class EventHandlerTests
{
    [Fact]
    public async Task LessonCompletedEventHandler_ShouldLogAndComplete()
    {
        var loggerMock = new Mock<ILogger<LessonCompletedEventHandler>>();
        var handler = new LessonCompletedEventHandler(loggerMock.Object);
        var notification = new LessonCompletedEvent(Guid.NewGuid(), Guid.NewGuid(), 85);

        await handler.Handle(notification, CancellationToken.None);

        loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task UserCreatedEventHandler_ShouldLogAndComplete()
    {
        var loggerMock = new Mock<ILogger<UserCreatedEventHandler>>();
        var handler = new UserCreatedEventHandler(loggerMock.Object);
        var notification = new UserCreatedEvent(Guid.NewGuid(), "test@test.com");

        await handler.Handle(notification, CancellationToken.None);

        loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}
