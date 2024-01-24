using System.Net;

namespace Core.Modules.HumanResources.Domain.Exceptions.Contracts;

internal class AlreadyBrokenContractException : BaseHttpException
{
    public AlreadyBrokenContractException(Guid brokenContractId) : base($"Found a already broken contract with id {brokenContractId}", HttpStatusCode.Conflict)
    { }
}