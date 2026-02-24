using Microsoft.AspNetCore.Identity;

namespace AiTutor.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public Guid DomainUserId { get; set; }
}
