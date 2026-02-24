using AiTutor.Application.Common.Exceptions;
using AiTutor.Domain.Exceptions;
using System.Net;
using System.Text.Json;

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
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message, errors) = exception switch
        {
            Application.Common.Exceptions.ValidationException validationEx =>
                (HttpStatusCode.BadRequest, "Validation failed",
                    (object)validationEx.Errors),
            NotFoundException notFoundEx =>
                (HttpStatusCode.NotFound, notFoundEx.Message, (object)new { }),
            ForbiddenAccessException =>
                (HttpStatusCode.Forbidden, "Access forbidden.", (object)new { }),
            DomainException domainEx =>
                (HttpStatusCode.BadRequest, domainEx.Message, (object)new { }),
            _ =>
                (HttpStatusCode.InternalServerError,
                    "An unexpected error occurred.", (object)new { })
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            status = (int)statusCode,
            message,
            errors
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
    }
}
