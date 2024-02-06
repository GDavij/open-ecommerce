using System.Net;

namespace Core.Modules.HumanResources.Domain.Exceptions.JobApplication;

internal class InvalidJobApplicationException : BaseHttpException
{
    public InvalidJobApplicationException(Guid invalidJobApplicationId) : base($"Found Invalid Job Application {invalidJobApplicationId}", HttpStatusCode.BadRequest)
    { }
}