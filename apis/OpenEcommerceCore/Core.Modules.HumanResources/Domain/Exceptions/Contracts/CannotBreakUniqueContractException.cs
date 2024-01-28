using System.Net;

namespace Core.Modules.HumanResources.Domain.Exceptions.Contracts;

internal class CannotBreakUniqueContractException : BaseHttpException
{
    public CannotBreakUniqueContractException(Guid collaboratorId) : base($"Cannot the unique Contract for Collaborator {collaboratorId}, delete action is suggested instead", HttpStatusCode.Forbidden)
    { }
}