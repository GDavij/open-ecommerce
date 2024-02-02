using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.Contracts.DeleteContract;
using FluentValidation;

namespace Core.Modules.HumanResources.Application.Http.Commands.Contracts.DeleteContract;

internal class DeleteContractCommandValidator : AbstractValidator<DeleteContractCommand>
{
    public DeleteContractCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("Id must not be empty");
    }
}