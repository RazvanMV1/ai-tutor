namespace AiTutor.Web.Models;

public record LoginRequest(string Email, string Password);

public record RegisterRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    int Role);

public record AuthResponse(string Token, UserInfo User);

public record UserInfo(Guid Id, string FullName, string Email, int Role);
