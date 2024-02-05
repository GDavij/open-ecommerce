using System.Net;

namespace Core.Modules.HumanResources.Domain.Exceptions.ContributionYear;

internal class InvalidYearOfContributionException : BaseHttpException
{
    public InvalidYearOfContributionException(int invalidYearOfContribution) : base($"Invalid year of contribution: {invalidYearOfContribution}", HttpStatusCode.BadRequest)
    { }
}