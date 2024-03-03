using Core.Modules.UserAccess.Domain.Contracts.Http.Queries.Administrators.ListAdministrators;
using FluentValidation;

namespace Core.Modules.UserAccess.Application.Http.Queries.Administrators.ListAdministrators;

internal class ListAdministratorsQueryValidator : AbstractValidator<ListAdministratorsQuery>
{
    public ListAdministratorsQueryValidator()
    {
        RuleFor(c => c.Page)
            .NotEmpty().WithMessage("Page must not be empty")
            .GreaterThanOrEqualTo(1).WithMessage("Page must be greater than or equal to 1");

        When(c => c.Email is not null, () =>
        {
            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("Email must not be empty")
                .MaximumLength(255).WithMessage("Email must have a maximum of 255 characters");
        });

        When(c => c.Period is not null, () =>
        {
            RuleFor(c => c.Period).ChildRules(p =>
            {
                p.RuleFor(p => p.Start)
                    .NotEmpty().WithMessage("Period start must not be empty")
                    .LessThanOrEqualTo(c => c.End).WithMessage("Period start must be less than or equal to period end");

                p.RuleFor(c => c.End)
                    .NotEmpty().WithMessage("Period end must not be empty")
                    .GreaterThanOrEqualTo(c => c.Start)
                    .WithMessage("Period end must be greater than or equal to period start");
            });
        });
    }
}