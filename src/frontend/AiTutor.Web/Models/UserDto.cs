namespace AiTutor.Web.Models;

public record LoginRequest(
    string Email,
    string Password);

public record RegisterRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    int Role);

// ⚠️ Role vine ca string "Admin" nu int!
// ⚠️ userId separat, nu în user object
public record AuthResponse(
    string Token,
    Guid UserId,
    string Email,
    string FirstName,
    string LastName,
    string Role)
{
    public string FullName => $"{FirstName} {LastName}";
}

public record RegisterResponse(
    Guid Id,
    string FullName,
    string Email,
    int Role);
