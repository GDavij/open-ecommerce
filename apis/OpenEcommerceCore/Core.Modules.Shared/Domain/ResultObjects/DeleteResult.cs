using System.Net;

namespace Core.Modules.Shared.Domain.ResultObjects;

public class DeleteResult
{
        
    public bool HasError { get; init; }
    public string? ErrorMessage { get; init; }
    public HttpStatusCode Code { get; init; }
    public string? Redirect { get; init; }

    private DeleteResult(bool hasError, string? errorMessage, HttpStatusCode code, string? redirect)
    {
        HasError = hasError;
        ErrorMessage = errorMessage;
        Code = code;
        Redirect = redirect;
    }

    public static DeleteResult CouldNotDelete(string errorMessage, HttpStatusCode statusCode)
    {
        return new DeleteResult(true, errorMessage, statusCode, null);
    }

    public static DeleteResult Delete(string redirect)
    {
        return new DeleteResult(false, null, HttpStatusCode.Accepted, redirect);
    }
}