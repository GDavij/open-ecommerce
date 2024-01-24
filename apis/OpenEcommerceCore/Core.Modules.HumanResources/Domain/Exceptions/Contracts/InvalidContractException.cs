using System.Net;

namespace Core.Modules.HumanResources.Domain.Exceptions.Contracts;

internal class InvalidContractException : BaseHttpException
{
    public InvalidContractException(Guid invalidContractId) : base($"Found a invalid contract with id {invalidContractId}", HttpStatusCode.BadRequest)
    { }
}