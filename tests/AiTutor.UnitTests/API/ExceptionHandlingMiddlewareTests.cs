using System.Net;
using AiTutor.API.Middleware;
using AiTutor.Application.Common.Exceptions;
using AiTutor.Domain.Exceptions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace AiTutor.UnitTests.API;

public class ExceptionHandlingMiddlewareTests
{
    private static async Task<(int statusCode, string body)> RunWith(Exception toThrow)
    {
        var ctx = new DefaultHttpContext();
        ctx.Response.Body = new MemoryStream();

        RequestDelegate next = _ => throw toThrow;
        var mw = new ExceptionHandlingMiddleware(next, NullLogger<ExceptionHandlingMiddleware>.Instance);

        await mw.InvokeAsync(ctx);

        ctx.Response.Body.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(ctx.Response.Body).ReadToEndAsync();
        return (ctx.Response.StatusCode, body);
    }

    [Fact]
    public async Task NoException_PassesThrough()
    {
        var ctx = new DefaultHttpContext();
        ctx.Response.Body = new MemoryStream();
        var called = false;
        RequestDelegate next = _ => { called = true; return Task.CompletedTask; };
        var mw = new ExceptionHandlingMiddleware(next, NullLogger<ExceptionHandlingMiddleware>.Instance);

        await mw.InvokeAsync(ctx);

        called.Should().BeTrue();
        ctx.Response.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task ValidationException_Returns400()
    {
        var ex = new ValidationException(new[]
        {
            new FluentValidation.Results.ValidationFailure("Name", "required")
        });
        var result = await RunWith(ex);

        result.statusCode.Should().Be((int)HttpStatusCode.BadRequest);
        result.body.Should().Contain("Validation failed");
    }

    [Fact]
    public async Task NotFoundException_Returns404()
    {
        var result = await RunWith(new NotFoundException("User", Guid.NewGuid()));
        result.statusCode.Should().Be((int)HttpStatusCode.NotFound);
        result.body.Should().Contain("User");
    }

    [Fact]
    public async Task ForbiddenAccessException_Returns403()
    {
        var result = await RunWith(new ForbiddenAccessException());
        result.statusCode.Should().Be((int)HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DomainException_Returns400()
    {
        var result = await RunWith(new DomainException("biz rule violated"));
        result.statusCode.Should().Be((int)HttpStatusCode.BadRequest);
        result.body.Should().Contain("biz rule violated");
    }

    [Fact]
    public async Task DbUpdateException_Returns400()
    {
        var result = await RunWith(new DbUpdateException("conflict"));
        result.statusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UnknownException_Returns500()
    {
        var result = await RunWith(new InvalidCastException("boom"));
        result.statusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        result.body.Should().Contain("unexpected");
    }
}
