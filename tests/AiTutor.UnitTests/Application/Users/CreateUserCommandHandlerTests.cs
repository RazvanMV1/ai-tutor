using AiTutor.Application.Features.Users.Commands.CreateUser;
using AiTutor.Domain.Enums;
using FluentAssertions;

namespace AiTutor.UnitTests.Application.Users;

public class CreateUserCommandHandlerTests
{
    private readonly TestDbContext _context;
    private readonly CreateUserCommandHandler _handler;

    public CreateUserCommandHandlerTests()
    {
        _context = TestDbContextFactory.Create();
        _handler = new CreateUserCommandHandler(_context);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateUser()
    {
        // Arrange
        var command = new CreateUserCommand(
            "John", "Doe", "john@example.com", "Password123!", UserRole.Student);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.FullName.Should().Be("John Doe");
        result.Data.Email.Should().Be("john@example.com");
        result.Data.Role.Should().Be(UserRole.Student);
        result.StatusCode.Should().Be(201);
    }

    [Fact]
    public async Task Handle_ShouldPersistUserInDatabase()
    {
        // Arrange
        var command = new CreateUserCommand(
            "John", "Doe", "john@example.com", "Password123!", UserRole.Student);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _context.Users.Should().HaveCount(1);
    }
}
