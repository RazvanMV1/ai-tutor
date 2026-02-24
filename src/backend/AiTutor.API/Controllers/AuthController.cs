using AiTutor.Application.Common.Interfaces;
using AiTutor.Application.Features.Users.Commands.CreateUser;
using AiTutor.Domain.Enums;
using AiTutor.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AiTutor.API.Controllers;

public class AuthController : BaseController
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtService _jwtService;
    private readonly IApplicationDbContext _context;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtService jwtService,
        IApplicationDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await Mediator.Send(new CreateUserCommand(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password,
            request.Role));

        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, new { message = result.Error });

        var identityUser = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            DomainUserId = result.Data!.Id
        };

        var identityResult = await _userManager.CreateAsync(identityUser, request.Password);
        if (!identityResult.Succeeded)
            return BadRequest(identityResult.Errors);

        return StatusCode(201, result.Data);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var identityUser = await _userManager.FindByEmailAsync(request.Email);
        if (identityUser is null)
            return Unauthorized(new { message = "Invalid credentials." });

        var result = await _signInManager
            .CheckPasswordSignInAsync(identityUser, request.Password, false);

        if (!result.Succeeded)
            return Unauthorized(new { message = "Invalid credentials." });

        var domainUser = _context.Users
            .FirstOrDefault(u => u.Id == identityUser.DomainUserId);

        if (domainUser is null)
            return Unauthorized(new { message = "User not found." });

        var token = _jwtService.GenerateToken(domainUser);

        return Ok(new
        {
            token,
            user = new
            {
                domainUser.Id,
                domainUser.FullName,
                Email = domainUser.Email.Value,
                domainUser.Role
            }
        });
    }
}

public record RegisterRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    UserRole Role);

public record LoginRequest(string Email, string Password);
