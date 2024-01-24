using System.Net;

namespace Core.Modules.HumanResources.Domain.Exceptions.Collaborators;

internal class AlreadyDeletedCollaboratorException : BaseHttpException 
{
    public AlreadyDeletedCollaboratorException(Guid collaboratorId) : base($"Found a already deleted collaborator with id {collaboratorId}", HttpStatusCode.Conflict)
    { }
}