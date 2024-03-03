using System.Net;

namespace Core.Modules.Shared.Domain.ResultObjects;

public class SearchResult<T> where T : class
{
    
    public bool HasError { get; init; }
    public string? ErrorMessage { get; init; }
    public HttpStatusCode Code { get; init; }
    public T? Result { get; init; }

    private SearchResult(bool hasError, string? errorMessage, HttpStatusCode code, T? result)
    {
        HasError = hasError;
        ErrorMessage = errorMessage;
        Code = code;
        Result = result;
    }

    public static SearchResult<T> CouldNotSearch(string errorMessage, HttpStatusCode statusCode)
    {
        return new SearchResult<T>(true, errorMessage, statusCode, null);
    }

    public static SearchResult<T> SearchFound(T result)
    {
        return new SearchResult<T>(false, null, HttpStatusCode.OK, result);
    }
    
}