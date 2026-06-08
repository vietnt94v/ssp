using System.Net;
using System.Text.Json;
using Ssp.Cmms.Application.Common.Exceptions;

namespace Ssp.Cmms.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
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
            await HandleAsync(context, ex);
        }
    }

    private async Task HandleAsync(HttpContext context, Exception ex)
    {
        var (status, code, message, details) = ex switch
        {
            ValidationException v => (
                HttpStatusCode.BadRequest,
                "VALIDATION_ERROR",
                v.Message,
                v.Errors.ToArray()),
            NotFoundException n => (
                HttpStatusCode.NotFound,
                "NOT_FOUND",
                n.Message,
                Array.Empty<string>()),
            BusinessRuleException b => (
                HttpStatusCode.Conflict,
                "BUSINESS_RULE",
                b.Message,
                Array.Empty<string>()),
            _ => (
                HttpStatusCode.InternalServerError,
                "INTERNAL_ERROR",
                "An unexpected error occurred.",
                Array.Empty<string>())
        };

        if (status == HttpStatusCode.InternalServerError)
        {
            _logger.LogError(ex, "Unhandled exception");
        }
        else
        {
            _logger.LogWarning(ex, "Handled exception: {Code}", code);
        }

        context.Response.StatusCode = (int)status;
        context.Response.ContentType = "application/json";

        var payload = JsonSerializer.Serialize(
            new { error = message, code, details },
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

        await context.Response.WriteAsync(payload);
    }
}
