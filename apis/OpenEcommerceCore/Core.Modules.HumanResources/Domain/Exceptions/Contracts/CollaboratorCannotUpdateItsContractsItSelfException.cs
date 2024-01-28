using System.Net;

namespace Core.Modules.HumanResources.Domain.Exceptions.Contracts;

internal class CollaboratorCannotUpdateItsContractsItSelfException : BaseHttpException
{
    public CollaboratorCannotUpdateItsContractsItSelfException() : base($"Human Resources Collaborator Cannot Update its contracts it self", HttpStatusCode.Forbidden)
    { }
}