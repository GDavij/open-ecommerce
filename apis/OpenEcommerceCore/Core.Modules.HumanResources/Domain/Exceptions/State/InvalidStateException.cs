using System.Net;

namespace Core.Modules.HumanResources.Domain.Exceptions.State;

internal class InvalidStateException : BaseHttpException
{
    public InvalidStateException(Guid stateId) : base($"Found invalid state with id {stateId}", HttpStatusCode.BadRequest)
    { }
}