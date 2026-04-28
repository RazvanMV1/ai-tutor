using AiTutor.Application.Common.Interfaces;
using AiTutor.Infrastructure.Identity;
using AiTutor.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.API.Controllers;

/// <summary>
/// Endpoint-uri administrative pentru operațiuni rare de mentenanță.
/// Toate endpoint-urile sunt protejate printr-un secret configurat
/// prin "Admin:MaintenanceSecret" în appsettings/env vars.
///
/// IMPORTANT: Acest controller este destinat operațiunilor manuale rare.
/// Endpoint-urile aici NU trebuie expuse fără secret valid.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AdminController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _config;
    private readonly ILogger<AdminController> _logger;

    public AdminController(
        ApplicationDbContext db,
        UserManager<ApplicationUser> userManager,
        IConfiguration config,
        ILogger<AdminController> logger)
    {
        _db = db;
        _userManager = userManager;
        _config = config;
        _logger = logger;
    }

    /// <summary>
    /// Resetează complet baza de date: șterge toate tabelele, recreează schema
    /// și rulează seed-ul de la zero. DESTRUCTIV — folosit doar manual.
    /// Necesită header X-Admin-Secret cu valoarea configurată.
    /// </summary>
    [HttpPost("reset-database")]
    public async Task<IActionResult> ResetDatabase()
    {
        var providedSecret = Request.Headers["X-Admin-Secret"].ToString();
        var expectedSecret = _config["Admin:MaintenanceSecret"];

        if (string.IsNullOrWhiteSpace(expectedSecret))
        {
            _logger.LogWarning("ResetDatabase called but Admin:MaintenanceSecret is not configured.");
            return StatusCode(503, new { error = "Maintenance endpoint not configured." });
        }

        if (!string.Equals(providedSecret, expectedSecret, StringComparison.Ordinal))
        {
            _logger.LogWarning("ResetDatabase called with invalid secret.");
            return Unauthorized(new { error = "Invalid admin secret." });
        }

        _logger.LogWarning("Resetting database — all data will be lost.");

        // Drop + recreate complet
        await _db.Database.EnsureDeletedAsync();
        await _db.Database.MigrateAsync();
        await SeedData.SeedAsync(_db, _userManager);

        _logger.LogInformation("Database reset and seeded successfully.");

        return Ok(new
        {
            message = "Database reset and seeded successfully.",
            timestamp = DateTime.UtcNow
        });
    }

    /// <summary>
    /// Health check simplu — confirmă că endpoint-ul admin există și DB-ul răspunde.
    /// Nu necesită secret. Util pentru smoke tests.
    /// </summary>
    [HttpGet("health")]
    public async Task<IActionResult> Health()
    {
        var canConnect = await _db.Database.CanConnectAsync();
        var userCount = canConnect ? await _db.Users.CountAsync() : 0;
        var identityUserCount = canConnect ? await _userManager.Users.CountAsync() : 0;

        return Ok(new
        {
            dbReachable = canConnect,
            domainUsers = userCount,
            identityUsers = identityUserCount,
            timestamp = DateTime.UtcNow
        });
    }
}
