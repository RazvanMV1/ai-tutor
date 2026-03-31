using AiTutor.Application.Common.Exceptions;
using FluentAssertions;
using FluentValidation.Results;
using Xunit;

namespace AiTutor.UnitTests.Application.Common;

public class ExceptionTests
{
    [Fact]
    public void ValidationException_ShouldGroupErrorsByProperty()
    {
        var failures = new List<ValidationFailure>
        {
            new ValidationFailure("Name", "Name is required."),
            new ValidationFailure("Name", "Name is too short."),
            new ValidationFailure("Email", "Email is required.")
        };

        var exception = new ValidationException(failures);

        exception.Errors.Should().ContainKey("Name");
        exception.Errors["Name"].Should().HaveCount(2);
        exception.Errors.Should().ContainKey("Email");
        exception.Errors["Email"].Should().HaveCount(1);
        exception.Message.Should().Be("One or more validation failures have occurred.");
    }

    [Fact]
    public void NotFoundException_ShouldContainEntityInfo()
    {
        var exception = new NotFoundException("User", Guid.NewGuid());

        exception.Message.Should().Contain("User");
    }

    [Fact]
    public void ForbiddenAccessException_ShouldBeCreatable()
    {
        var exception = new ForbiddenAccessException();

        exception.Should().NotBeNull();
    }
}
