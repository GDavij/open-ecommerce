using Core.Modules.UserAccess.Domain.Contracts.Http.Commands.Administrators.UpdateAdministrator;
using FluentValidation;

namespace Core.Modules.UserAccess.Application.Http.Commands.Administrators.UpdateAdministrator;

internal class UpdateAdministratorCommandValidator : AbstractValidator<UpdateAdministratorCommand>
{
    public UpdateAdministratorCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("Id must not be empty");

        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("E-mail must not be empty")
            .MaximumLength(255).WithMessage("E-mail must not be greater than 255 characters")
            .EmailAddress().WithMessage("E-mail must be valid");

        When(c => c.Password is not null, () =>
        {
            RuleFor(c => c.Password)
                .MaximumLength(512).WithMessage("Password must not be greater than 512 characters");
        });

    }
}