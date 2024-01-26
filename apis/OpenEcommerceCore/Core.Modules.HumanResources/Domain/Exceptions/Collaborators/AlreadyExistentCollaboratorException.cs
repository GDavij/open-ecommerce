using System.Net;

namespace Core.Modules.HumanResources.Domain.Exceptions.Collaborators;

internal class AlreadyExistentCollaboratorException : BaseHttpException
{
    public AlreadyExistentCollaboratorException(string collaboratorEmail, string collaboratorPhone) : base($"already found a collaborator with email {collaboratorEmail} or phone {collaboratorPhone}", HttpStatusCode.Conflict)
    { }
}