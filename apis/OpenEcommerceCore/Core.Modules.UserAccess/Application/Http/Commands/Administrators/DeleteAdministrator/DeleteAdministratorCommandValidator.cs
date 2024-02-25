using Core.Modules.UserAccess.Domain.Contracts.Http.Commands.Administrators.DeleteAdministrator;
using FluentValidation;

namespace Core.Modules.UserAccess.Application.Http.Commands.Administrators.DeleteAdministrator;

internal class DeleteAdministratorCommandValidator : AbstractValidator<DeleteAdministratorCommand>
{
    public DeleteAdministratorCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("Id must not be empty");
    }
}