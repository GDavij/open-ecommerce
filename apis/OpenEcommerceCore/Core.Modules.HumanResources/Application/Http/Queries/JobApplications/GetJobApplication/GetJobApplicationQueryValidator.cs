using Core.Modules.HumanResources.Domain.Contracts.Http.Queries.JobApplications.GetJobApplication;
using FluentValidation;

namespace Core.Modules.HumanResources.Application.Http.Queries.JobApplications.GetJobApplication;

internal class GetJobApplicationQueryValidator : AbstractValidator<GetJobApplicationQuery>
{
    public GetJobApplicationQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id must not be empty");
    }
}