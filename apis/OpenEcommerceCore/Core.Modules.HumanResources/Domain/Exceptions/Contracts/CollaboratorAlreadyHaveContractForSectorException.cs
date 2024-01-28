using System.Net;
using Core.Modules.HumanResources.Domain.Entities;
using Core.Modules.Shared.Domain.BusinessHierarchy;

namespace Core.Modules.HumanResources.Domain.Exceptions.Contracts;

internal class CollaboratorAlreadyHaveContractForSectorException : BaseHttpException
{
    public CollaboratorAlreadyHaveContractForSectorException(ECollaboratorSector sector) : base($"Collaborator already have a contract for sector {sector:G}", HttpStatusCode.Conflict)
    { }
}