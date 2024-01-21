using System.Net;

namespace Core.Modules.HumanResources.Domain.Exceptions.Collaborators;

internal class InvalidCollaboratorException : BaseHttpException
{
    public InvalidCollaboratorException(Guid invalidCollaboratorId) : base($"Found a Invalid collaborator with Id {invalidCollaboratorId}", HttpStatusCode.BadRequest)
    { }
}