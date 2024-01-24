using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.BreakContract;
using FluentValidation;

namespace Core.Modules.HumanResources.Application.Http.Commands.BreakContract;

internal class BreakContractCommandValidator : AbstractValidator<BreakContractCommand>
{
    public BreakContractCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("Id must not be empty");
    }
}