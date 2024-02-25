using System.Net;

namespace Core.Modules.Shared.Domain.ResultObjects;

public class UpdateResult
{
    
    public bool HasError { get; init; }
    public string? ErrorMessage { get; init; }
    public HttpStatusCode Code { get; init; }
    public string? Resource { get; init; }

    private UpdateResult(bool hasError, string? errorMessage, HttpStatusCode code, string? resource)
    {
        HasError = hasError;
        ErrorMessage = errorMessage;
        Code = code;
        Resource = resource;
    }

    public static UpdateResult CouldNotUpdate(string errorMessage, HttpStatusCode statusCode)
    {
        return new UpdateResult(true, errorMessage, statusCode, null);
    }

    public static UpdateResult Update(string resource)
    {
        return new UpdateResult(false, null, HttpStatusCode.Created, resource);
    }
}