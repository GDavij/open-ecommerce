using Core.Modules.HumanResources.Application.Http.SharedValidators;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.Contracts.AddContracts;
using FluentValidation;

namespace Core.Modules.HumanResources.Application.Http.Commands.Contracts.AddContracts;

internal class AddContractsCommandValidator : AbstractValidator<AddContractsCommand>
{
    public AddContractsCommandValidator()
    {
        RuleFor(c => c.CollaboratorId)
            .NotEmpty().WithMessage("Collaborator Id must not be empty");

        RuleFor(c => c.Contracts)
            .NotEmpty().WithMessage("At least one contract must be sent to be added")
            .Must(ContractValidators.HaveValidContributionYears).WithMessage("Contract Contribution Years must be valid")
            .Must(ContractValidators.HaveOnlyOneContractForASector).WithMessage("Collaborators must have a unique contract for a sector");

        RuleForEach(c => c.Contracts).ChildRules(c =>
        {
            c.RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Contract Name must not be empty")
                .MaximumLength(255).WithMessage("Contract Name must have a maximum of 255 characters");

            c.RuleFor(c => c.Sector)
                .NotNull().WithMessage("Contract Sector must not be null")
                .IsInEnum().WithMessage("Contract Sector is not a valid one");

            c.RuleFor(c => c.StartDate)
                .NotEmpty().WithMessage("Contract Start Date must not be empty")
                .LessThan(c => c.EndDate.AddMonths(2));

            c.RuleFor(c => c.EndDate)
                .NotEmpty().WithMessage("Contract End Date must not be empty")
                .GreaterThan(c => c.StartDate);

            c.RuleFor(c => c.MonthlySalary)
                .NotEmpty().WithMessage("Contract Monthly Salary must not be empty")
                .GreaterThan(1100).WithMessage("Contract Monthly Salary must be greater than 1100$");

            c.RuleFor(c => c.Broken)
                .NotNull().WithMessage("Contract Broken must not be null");
        });
    }
}