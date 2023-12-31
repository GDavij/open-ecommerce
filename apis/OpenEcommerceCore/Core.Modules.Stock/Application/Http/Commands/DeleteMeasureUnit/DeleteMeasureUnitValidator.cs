using Core.Modules.Stock.Domain.Contracts.Http.Commands.DeleteMeasureUnit;
using FluentValidation;

namespace Core.Modules.Stock.Application.Http.Commands.DeleteMeasureUnit;

internal class DeleteMeasureUnitValidator : AbstractValidator<DeleteMeasureUnitCommand>
{
    public DeleteMeasureUnitValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("Id must not be empty");
    }
}