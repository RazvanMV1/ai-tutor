using AiTutor.Application.Common.Exceptions;
using AiTutor.Domain.Exceptions;
using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace AiTutor.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred: {ExceptionType}",
                ex.GetType().FullName);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        int statusCode;
        object response;

        if (exception is ValidationException validationEx)
        {
            statusCode = (int)HttpStatusCode.BadRequest;
            response = new
            {
                status = statusCode,
                message = "Validation failed",
                errors = validationEx.Errors
            };
        }
        else if (exception is NotFoundException notFoundEx)
        {
            statusCode = (int)HttpStatusCode.NotFound;
            response = new
            {
                status = statusCode,
                message = notFoundEx.Message,
                errors = new { }
            };
        }
        else if (exception is ForbiddenAccessException)
        {
            statusCode = (int)HttpStatusCode.Forbidden;
            response = new
            {
                status = statusCode,
                message = "Access forbidden.",
                errors = new { }
            };
        }
        else if (exception is DomainException domainEx)
        {
            statusCode = (int)HttpStatusCode.BadRequest;
            response = new
            {
                status = statusCode,
                message = domainEx.Message,
                errors = new { }
            };
        }
        else if (exception is DbUpdateException dbEx)
        {
            statusCode = (int)HttpStatusCode.BadRequest;
            response = new
            {
                status = statusCode,
                message = dbEx.InnerException?.Message ?? dbEx.Message,
                errors = new { }
            };
        }
        else
        {
            statusCode = (int)HttpStatusCode.InternalServerError;
            response = new
            {
                status = statusCode,
                message = "An unexpected error occurred.",
                errors = new { }
            };
        }

        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsync(JsonSerializer.Serialize(response,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
    }
}
