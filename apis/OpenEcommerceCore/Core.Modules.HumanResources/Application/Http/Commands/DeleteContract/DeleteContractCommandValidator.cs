using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.DeleteContract;
using FluentValidation;

namespace Core.Modules.HumanResources.Application.Http.Commands.DeleteContract;

internal class DeleteContractCommandValidator : AbstractValidator<DeleteContractCommand>
{
    public DeleteContractCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("Id must not be empty");
    }
}