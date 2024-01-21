using System.Net;

namespace Core.Modules.HumanResources.Domain.Exceptions.Collaborators;

internal class MissingAdministrativePrivilegesException : BaseHttpException
{
    public MissingAdministrativePrivilegesException(string action = "action") : base($"Missing Administrative Privileges to Perform {action}", HttpStatusCode.Forbidden)
    { }
}