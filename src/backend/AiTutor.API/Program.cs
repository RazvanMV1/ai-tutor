using System.Text;
using AiTutor.Application;
using AiTutor.Infrastructure;
using AiTutor.Infrastructure.Persistence;
using AiTutor.API.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Application + Infrastructure
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AiTutor API",
        Version = "v1",
        Description = "AI-Enhanced Tutor API for students"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Auto migrate + seed on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await db.Database.MigrateAsync();
    await SeedDefaultDataAsync(db);
}

app.Run();

static async Task SeedDefaultDataAsync(ApplicationDbContext db)
{
    // Seed default subjects if none exist
    if (!db.Subjects.Any())
    {
        var subjects = new[]
        {
            AiTutor.Domain.Entities.Subject.Create(
                "Matematică",
                "Aritmetică, algebră, geometrie și analiză matematică pentru toate nivelurile.",
                AiTutor.Domain.Enums.SubjectType.Mathematics),
            AiTutor.Domain.Entities.Subject.Create(
                "Limba Română",
                "Gramatică, lectură, scriere creativă și literatura română.",
                AiTutor.Domain.Enums.SubjectType.Romanian),
            AiTutor.Domain.Entities.Subject.Create(
                "Informatică",
                "Algoritmi, programare, structuri de date și gândire computațională.",
                AiTutor.Domain.Enums.SubjectType.Informatics),
        };

        db.Subjects.AddRange(subjects);
        await db.SaveChangesAsync();
    }
}

// Necesar pentru IntegrationTests
public partial class Program { }
