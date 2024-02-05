using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.Contracts.AddWorkHourToContributionYear;
using FluentValidation;

namespace Core.Modules.HumanResources.Application.Http.Commands.Contracts.AddWorkHourToContributionYear;

internal class AddWorkHourToContributionYearCommandValidator : AbstractValidator<AddWorkHourToContributionYearCommand>
{
    public AddWorkHourToContributionYearCommandValidator()
    {
        RuleFor(x => x.ContractId)
            .NotEmpty().WithMessage("Contract Id must not be empty");

        RuleFor(x => x.WorkHour)
            .NotEmpty().WithMessage("Work Hour must not be empty")
            .ChildRules(x =>
            {
                x.RuleFor(x => x.Date)
                    .NotEmpty().WithMessage("Work Hour Date must not be empty")
                    .LessThanOrEqualTo(DateTime.UtcNow)
                    .WithMessage("Work Hour Date must be less than or equal today");

                x.RuleFor(x => x.Start)
                    .NotEmpty().WithMessage("Work Hour Start must not be empty")
                    .LessThan(x => x.End).WithMessage("Start Date must be less than end date");

                x.RuleFor(x => x.End)
                    .NotEmpty().WithMessage("End Date must not be empty")
                    .GreaterThan(x => x.Start).WithMessage("End Date must be greater than start date");
            });
    }
}