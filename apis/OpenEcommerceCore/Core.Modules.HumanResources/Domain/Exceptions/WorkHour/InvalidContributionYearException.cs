using System.Net;

namespace Core.Modules.HumanResources.Domain.Exceptions.WorkHour;

internal class InvalidContributionYearException : BaseHttpException
{
    public InvalidContributionYearException(Guid invalidContributionYearId) : base($"Found invalid contribution year {invalidContributionYearId}", HttpStatusCode.BadRequest)
    { }
}