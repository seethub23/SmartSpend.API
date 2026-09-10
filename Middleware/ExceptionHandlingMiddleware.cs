using Microsoft.AspNetCore.Mvc;

namespace SmartSpend.API.Middleware;

/// <summary>
/// Converts exceptions that escape the request pipeline into RFC 7807 ProblemDetails responses.
/// </summary>
public sealed class ExceptionHandlingMiddleware
{
    private const string CorrelationIdHeader = "X-Correlation-ID";
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    // RequestDelegate is the *next link* in the middleware chain.
    // HttpContext contains this request, response, user, headers, and per-request items.
    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Reuse the caller's correlation ID or create one for this request.
        var correlationId = context.Request.Headers[CorrelationIdHeader].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(correlationId))
        {
            correlationId = Guid.NewGuid().ToString("N");
        }

        context.Response.Headers[CorrelationIdHeader] = correlationId;

        // Every log written in this scope can include CorrelationId in its structured properties.
        using (_logger.BeginScope(new Dictionary<string, object> { ["CorrelationId"] = correlationId }))
        {
            try
            {
                await _next(context); // Continue to the next middleware/controller.
            }
            catch (Exception exception) when (context.Response.HasStarted)
            {
                _logger.LogWarning(exception,
                    "The response has already started; an error response cannot be written. CorrelationId: {CorrelationId}",
                    correlationId);
                throw;
            }
            catch (Exception exception)
            {
                await WriteProblemDetailsAsync(context, exception, correlationId);
            }
        }
    }

    private async Task WriteProblemDetailsAsync(
        HttpContext context,
        Exception exception,
        string correlationId)
    {
        var (statusCode, title, detail) = exception switch
        {
            ResourceNotFoundException => (StatusCodes.Status404NotFound, "Resource not found", exception.Message),
            DomainValidationException => (StatusCodes.Status400BadRequest, "Invalid request", exception.Message),
            ApiException apiException => (apiException.StatusCode, "Request failed", apiException.Message),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred", "An unexpected server error occurred.")
        };

        _logger.LogError(exception,
            "Unhandled exception while processing {Method} {Path}. CorrelationId: {CorrelationId}",
            context.Request.Method, context.Request.Path, correlationId);

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        };
        problem.Extensions["correlationId"] = correlationId;

        context.Response.Clear();
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";
        context.Response.Headers[CorrelationIdHeader] = correlationId;
        await context.Response.WriteAsJsonAsync(problem);
    }
}
