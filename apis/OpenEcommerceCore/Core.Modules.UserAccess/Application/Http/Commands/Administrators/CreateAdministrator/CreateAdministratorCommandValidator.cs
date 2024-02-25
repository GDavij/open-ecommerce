using Core.Modules.UserAccess.Domain.Contracts.Http.Commands.Administrators.CreateAdministrator;
using FluentValidation;

namespace Core.Modules.UserAccess.Application.Http.Commands.Administrators.CreateAdministrator;

internal class CreateAdministratorCommandValidator : AbstractValidator<CreateAdministratorCommand>
{
    public CreateAdministratorCommandValidator()
    {
        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("E-mail must not be empty")
            .MaximumLength(255).WithMessage("E-mail must not be greater than 255 characters")
            .EmailAddress().WithMessage("E-mail must be valid");


        RuleFor(c => c.Password)
            .NotEmpty().WithMessage("Password must not be empty")
            .MaximumLength(512).WithMessage("Password must not be greater than 512 characters");
        
    }
}