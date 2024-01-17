using System.Net;

namespace Core.Modules.HumanResources.Domain.Exceptions;

internal class AlreadyExistentCollaboratorException : BaseHttpException
{
    public AlreadyExistentCollaboratorException(string collaboratorEmail) : base($"already found a collaborator with email {collaboratorEmail}", HttpStatusCode.Conflict)
    { }
}