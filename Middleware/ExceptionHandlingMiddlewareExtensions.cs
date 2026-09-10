namespace SmartSpend.API.Middleware;

public static class ExceptionHandlingMiddlewareExtensions
{
    /// <summary>Registers global exception handling near the start of the HTTP pipeline.</summary>
    public static IApplicationBuilder UseCustomExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
