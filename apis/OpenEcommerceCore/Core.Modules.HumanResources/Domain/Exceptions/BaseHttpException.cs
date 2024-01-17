using System.Net;

namespace Core.Modules.HumanResources.Domain.Exceptions;

internal class BaseHttpException : Exception
{
    public int StatusCode { get; init; }

    protected BaseHttpException(string message, HttpStatusCode statusCode) : base(message)
    {
        StatusCode = (int)statusCode;
    }
}