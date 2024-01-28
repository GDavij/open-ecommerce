using System.Net;

namespace Core.Modules.HumanResources.Domain.Exceptions.Collaborators;

internal class AdminCannotHaveContractsException : BaseHttpException
{
    public AdminCannotHaveContractsException() : base("Admins cannot have contracts attached to then", HttpStatusCode.Forbidden)
    { }
}