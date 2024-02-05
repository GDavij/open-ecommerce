using System.Net;

namespace Core.Modules.HumanResources.Domain.Exceptions.ContributionYear;

internal class AlreadyRegisteredWorkHourException : BaseHttpException
{
    public AlreadyRegisteredWorkHourException(DateTime date) : base($"Found a already registered Work Hour at date {date}", HttpStatusCode.Conflict)
    { }
}