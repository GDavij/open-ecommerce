using System.Net;
using Core.Modules.Shared.Domain.BusinessHierarchy;

namespace Core.Modules.HumanResources.Domain.Exceptions.JobApplication;

internal class AlreadyExistentJobApplicationForSector : BaseHttpException
{
    public AlreadyExistentJobApplicationForSector(ECollaboratorSector sector, string email, string phone) : base($"Found a already existent job application for sector {sector:G} with email {email} or phone {phone}", HttpStatusCode.Conflict)
    { }
}