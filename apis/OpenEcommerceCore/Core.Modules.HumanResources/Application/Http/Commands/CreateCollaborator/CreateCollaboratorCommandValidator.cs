using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.CreateCollaborator;
using FluentValidation;

namespace Core.Modules.HumanResources.Application.Http.Commands.CreateCollaborator;

public class CreateCollaboratorCommandValidator : AbstractValidator<CreateCollaboratorCommand>
{
    public CreateCollaboratorCommandValidator()
    {
        RuleFor(c => c.FirstName)
            .NotEmpty().WithMessage("First Name must not be empty")
            .MaximumLength(255).WithMessage("First Name must have a maximum of 255 characters");

        RuleFor(c => c.LastName)
            .MaximumLength(255).When(c => !string.IsNullOrEmpty(c.LastName)).WithMessage("Last Name must have a maximum of 255 characters when not null or empty");

        RuleFor(c => c.Description)
            .NotEmpty().WithMessage("Description must not be empty")
            .MaximumLength(2048).WithMessage("Description must have a maximum of 2048 characters");

        RuleFor(c => c.Age)
            .NotEmpty().WithMessage("Age must not be empty")
            .GreaterThan(12).WithMessage("Age must not be greater 12");
        
        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("Email must not be empty")
            .MaximumLength(255).WithMessage("Email must have a maximum of 255 characters");

        RuleFor(c => c.Phone)
            .NotEmpty().WithMessage("Phone must not be empty")
            .MaximumLength(22).WithMessage("Phone must have a maximum of 22 characters");

        RuleFor(c => c.Contracts)
            .Must(HaveValidContributionYears).WithMessage("Contract Contribution Years must be valid");
        
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
                .GreaterThan(1100).WithMessage("Contract Monthly Salary must not be empty");

            c.RuleFor(c => c.Broken)
                .NotEmpty().WithMessage("Contract Broken must not be empty");
        });
    }

    //TODO: Reduce Resource Usage in this method
    private bool HaveValidContributionYears(List<CreateCollaboratorCommand.Contract> contracts)
    {
        foreach (var contract in contracts)
        {
            foreach (var contributionYear in contract.ContributionsYears)
            {
                if (contributionYear.Year < contract.StartDate.Year || contributionYear.Year > contract.EndDate.Year)
                {
                    return false;
                }

                foreach (var workHour in contributionYear.WorkHours)
                {
                    if (workHour.Date.Year < contract.StartDate.Year || workHour.Date.Year > contract.EndDate.Year)
                    {
                        return false;
                    }

                    if (workHour.Start.Subtract(workHour.End).TotalHours >= 18)
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }
}
