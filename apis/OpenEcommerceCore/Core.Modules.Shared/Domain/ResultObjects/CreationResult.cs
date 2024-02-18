using System.Net;
using System.Text.Json.Serialization;

namespace Core.Modules.Shared.Domain.ResultObjects;

public class CreationResult
{
    public bool HasError { get; init; }
    public string? ErrorMessage { get; init; }
    public HttpStatusCode Code { get; init; }
    public string? Resource { get; init; }

    private CreationResult(bool hasError, string? errorMessage, HttpStatusCode code, string? resource)
    {
        HasError = hasError;
        ErrorMessage = errorMessage;
        Code = code;
        Resource = resource;
    }

    public static CreationResult CouldNotCreate(string errorMessage, HttpStatusCode statusCode)
    {
        return new CreationResult(true, errorMessage, statusCode, null);
    }

    public static CreationResult Create(string resource)
    {
        return new CreationResult(false, null, HttpStatusCode.Created, resource);
    }
}
