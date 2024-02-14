using System.Net;

namespace Core.Modules.HumanResources.Domain.Exceptions.State;

internal class AlreadyExistentStateException : BaseHttpException
{
    public AlreadyExistentStateException(string name, string shortName) : base($"Found already existent state with Name {name} or ShortName {shortName}", HttpStatusCode.Conflict)
    { }
}