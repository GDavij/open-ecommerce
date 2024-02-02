using Core.Modules.HumanResources.Domain.Contracts.Http.Queries.JobApplications.SearchJobApplications;
using FluentValidation;

namespace Core.Modules.HumanResources.Application.Http.Queries.JobApplications.SearchJobApplications;

internal class SearchJobApplicationsQueryValidator : AbstractValidator<SearchJobApplicationsQuery>
{
    public SearchJobApplicationsQueryValidator()
    {
        RuleFor(x => x.SearchTerm)
            .MaximumLength(255).WithMessage("Search term must not be greater than 255 characters");

        RuleFor(x => x.ProcessStep)
            .IsInEnum().When(x => x.ProcessStep is not null).WithMessage("Process step must be valid");

        RuleFor(x => x.Sector)
            .IsInEnum().When(x => x.Sector is not null).WithMessage("Sector must be valid");
    }
}