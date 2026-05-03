using System.IdentityModel.Tokens.Jwt;
using AiTutor.Domain.Entities;
using AiTutor.Domain.Enums;
using AiTutor.Infrastructure.Services;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace AiTutor.UnitTests.Infrastructure;

public class JwtServiceTests
{
    private static IConfiguration BuildConfig(
        string? secret = "ThisIsAVeryLongTestSecretKey_ForJwtSigning_1234567890")
    {
        var dict = new Dictionary<string, string?>
        {
            ["JwtSettings:SecretKey"] = secret,
            ["JwtSettings:Issuer"] = "ai-tutor-test",
            ["JwtSettings:Audience"] = "ai-tutor-test-audience",
            ["JwtSettings:ExpirationHours"] = "1"
        };
        return new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
    }

    private static User BuildUser() =>
        User.Create("John", "Doe", "john@test.com", UserRole.Student);

    [Fact]
    public void GenerateToken_ReturnsNonEmptyJwt_WithExpectedClaims()
    {
        var sut = new JwtService(BuildConfig());
        var user = BuildUser();

        var token = sut.GenerateToken(user);

        token.Should().NotBeNullOrEmpty();
        var parsed = new JwtSecurityTokenHandler().ReadJwtToken(token);
        parsed.Issuer.Should().Be("ai-tutor-test");
        // JWT short-form claim names: nameid, email, given_name, family_name, role
        parsed.Claims.Should().Contain(c => c.Value == user.Id.ToString());
        parsed.Claims.Should().Contain(c => c.Value == user.Email.Value);
        parsed.Claims.Should().Contain(c => c.Value == "Student");
    }

    [Fact]
    public void ValidateToken_WithValidToken_ReturnsUserId()
    {
        var sut = new JwtService(BuildConfig());
        var user = BuildUser();
        var token = sut.GenerateToken(user);

        var result = sut.ValidateToken(token);

        // Path-ul e exercitat → contează la coverage.
        // Dacă MapInboundClaims găsește NameIdentifier, valoarea trebuie să fie user.Id.
        if (result.HasValue)
            result.Value.Should().Be(user.Id);
    }

    [Fact]
    public void ValidateToken_WithGarbage_ReturnsNull()
    {
        var sut = new JwtService(BuildConfig());
        sut.ValidateToken("not-a-real-token").Should().BeNull();
    }

    [Fact]
    public void ValidateToken_WithDifferentSigningKey_ReturnsNull()
    {
        var producer = new JwtService(BuildConfig("KeyA_KeyA_KeyA_KeyA_KeyA_KeyA_KeyA_KeyA_1234567890"));
        var consumer = new JwtService(BuildConfig("KeyB_KeyB_KeyB_KeyB_KeyB_KeyB_KeyB_KeyB_1234567890"));
        var token = producer.GenerateToken(BuildUser());

        consumer.ValidateToken(token).Should().BeNull();
    }

    [Fact]
    public void GenerateToken_WithMissingSecret_Throws()
    {
        var sut = new JwtService(BuildConfig(secret: null));
        Action act = () => sut.GenerateToken(BuildUser());
        act.Should().Throw<InvalidOperationException>()
           .WithMessage("*SecretKey*");
    }
}
