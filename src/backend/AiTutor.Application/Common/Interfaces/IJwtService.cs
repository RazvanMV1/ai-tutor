using AiTutor.Domain.Entities;

namespace AiTutor.Application.Common.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
    Guid? ValidateToken(string token);
}
