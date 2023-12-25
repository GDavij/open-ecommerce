using Core.Modules.Stock.Domain.Contracts.Http.Commands.CreateMeasureUnit;
using FluentValidation;

namespace Core.Modules.Stock.Application.Http.Commands.CreateMeasureUnit;

internal class CreateMeasureUnitCommandValidator : AbstractValidator<CreateMeasureUnitCommand>
{
    public CreateMeasureUnitCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Measure Unit Name must not be empty")
            .MaximumLength(128).WithMessage("Measure Unit Name must have a maximum of 128 characters");

        RuleFor(c => c.ShortName)
            .MaximumLength(40).When(c => c.ShortName is not null).WithMessage("Measure Unit ShortName Must have a maximum of 40 characters when not null");

        RuleFor(c => c.Symbol)
            .NotEmpty().WithMessage("Measure Unit Symbol must not be empty")
            .MaximumLength(4).WithMessage("Measure Unit Symbol must have a maximum of 4 characters");
    }
}