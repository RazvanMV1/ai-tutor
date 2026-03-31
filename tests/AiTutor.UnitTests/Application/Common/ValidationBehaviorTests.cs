using AiTutor.Application.Common.Behaviors;
using AiTutor.Application.Common.Models;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;
using Xunit;
using ValidationException = AiTutor.Application.Common.Exceptions.ValidationException;

namespace AiTutor.UnitTests.Application.Common;

public class ValidationBehaviorTests
{
    public record TestCommand(string Name) : IRequest<Result<string>>;

    // ── No validators ──────────────────────────────────────────────────
    [Fact]
    public async Task ValidationBehavior_WithNoValidators_ShouldCallNext()
    {
        var validators = Enumerable.Empty<IValidator<TestCommand>>();
        var behavior = new ValidationBehavior<TestCommand, Result<string>>(validators);
        var expectedResult = Result<string>.Success("ok");

        var result = await behavior.Handle(
            new TestCommand("test"),
            () => Task.FromResult(expectedResult),
            CancellationToken.None);

        result.Should().Be(expectedResult);
    }

    // ── Valid request ──────────────────────────────────────────────────
    [Fact]
    public async Task ValidationBehavior_WithValidRequest_ShouldCallNext()
    {
        var validatorMock = new Mock<IValidator<TestCommand>>();
        validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<TestCommand>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var validators = new List<IValidator<TestCommand>> { validatorMock.Object };
        var behavior = new ValidationBehavior<TestCommand, Result<string>>(validators);
        var expectedResult = Result<string>.Success("ok");

        var result = await behavior.Handle(
            new TestCommand("test"),
            () => Task.FromResult(expectedResult),
            CancellationToken.None);

        result.Should().Be(expectedResult);
    }

    // ── Invalid request ────────────────────────────────────────────────
    [Fact]
    public async Task ValidationBehavior_WithInvalidRequest_ShouldThrowValidationException()
    {
        var validatorMock = new Mock<IValidator<TestCommand>>();
        validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<TestCommand>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("Name", "Name is required.")
            }));

        var validators = new List<IValidator<TestCommand>> { validatorMock.Object };
        var behavior = new ValidationBehavior<TestCommand, Result<string>>(validators);

        var act = async () => await behavior.Handle(
            new TestCommand(""),
            () => Task.FromResult(Result<string>.Success("ok")),
            CancellationToken.None);

        var exception = await act.Should().ThrowAsync<ValidationException>();
        exception.Which.Errors.Should().ContainKey("Name");
    }
}
