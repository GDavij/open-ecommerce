using Core.Modules.Stock.Domain.Contracts.Http.Commands.DeleteMeasureUnit;
using FluentValidation;

namespace Core.Modules.Stock.Application.Http.Commands.DeleteMeasureUnit;

internal class DeleteMeasureUnitCommandValidator : AbstractValidator<DeleteMeasureUnitCommand>
{
    public DeleteMeasureUnitCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("Id must not be empty");
    }
}