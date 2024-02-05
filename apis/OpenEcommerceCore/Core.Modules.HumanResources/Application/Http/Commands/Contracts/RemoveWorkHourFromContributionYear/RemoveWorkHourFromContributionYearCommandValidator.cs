using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.Contracts.RemoveWorkHourFromContributionYear;
using FluentValidation;

namespace Core.Modules.HumanResources.Application.Http.Commands.Contracts.RemoveWorkHourFromContributionYear;

internal class RemoveWorkHourFromContributionYearCommandValidator : AbstractValidator<RemoveWorkHourFromContributionYearCommand>
{
    public RemoveWorkHourFromContributionYearCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id must not be empty");
    }
}