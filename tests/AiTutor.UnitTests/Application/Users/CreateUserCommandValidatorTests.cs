using AiTutor.Application.Features.Users.Commands.CreateUser;
using AiTutor.Domain.Enums;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace AiTutor.UnitTests.Application.Users;

public class CreateUserCommandValidatorTests
{
    private readonly CreateUserCommandValidator _validator;

    public CreateUserCommandValidatorTests()
    {
        _validator = new CreateUserCommandValidator();
    }

    [Fact]
    public async Task Validate_WithValidCommand_ShouldNotHaveErrors()
    {
        // Arrange
        var command = new CreateUserCommand(
            "John", "Doe", "john@example.com", "Password123!", UserRole.Student);

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Validate_WithEmptyFirstName_ShouldHaveError()
    {
        // Arrange
        var command = new CreateUserCommand(
            "", "Doe", "john@example.com", "Password123!", UserRole.Student);

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    [Fact]
    public async Task Validate_WithInvalidEmail_ShouldHaveError()
    {
        // Arrange
        var command = new CreateUserCommand(
            "John", "Doe", "notanemail", "Password123!", UserRole.Student);

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Theory]
    [InlineData("short")]
    [InlineData("nouppercase1!")]
    [InlineData("NoNumber!")]
    public async Task Validate_WithInvalidPassword_ShouldHaveError(string password)
    {
        // Arrange
        var command = new CreateUserCommand(
            "John", "Doe", "john@example.com", password, UserRole.Student);

        // Act
        var result = await _validator.TestValidateAsync(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
}
