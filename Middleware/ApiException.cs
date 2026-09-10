namespace SmartSpend.API.Middleware;

/// <summary>
/// A business/domain error whose HTTP status is safe to expose to an API client.
/// Use this instead of throwing a plain Exception for expected failures.
/// </summary>
public class ApiException : Exception
{
    public ApiException(string message, int statusCode)
        : base(message)
    {
        StatusCode = statusCode;
    }

    public int StatusCode { get; }
}

public sealed class ResourceNotFoundException : ApiException
{
    public ResourceNotFoundException(string message) : base(message, StatusCodes.Status404NotFound) { }
}

public sealed class DomainValidationException : ApiException
{
    public DomainValidationException(string message) : base(message, StatusCodes.Status400BadRequest) { }
}
