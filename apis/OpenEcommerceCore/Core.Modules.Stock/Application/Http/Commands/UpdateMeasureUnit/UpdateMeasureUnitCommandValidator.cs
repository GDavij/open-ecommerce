using Core.Modules.Stock.Domain.Contracts.Http.Commands.UpdateMeasureUnit;
using FluentValidation;

namespace Core.Modules.Stock.Application.Http.Commands.UpdateMeasureUnit;

internal class UpdateMeasureUnitCommandValidator : AbstractValidator<UpdateMeasureUnitCommand>
{
    public UpdateMeasureUnitCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("Id must not be null");

        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Name must not be empty")
            .MaximumLength(128).WithMessage("Name must not be higher than 128 characters");

        RuleFor(c => c.ShortName)
            .MaximumLength(40).When(c => !string.IsNullOrEmpty(c.ShortName)).WithMessage("ShortName must not be higher than 40 characters when not null");

        RuleFor(c => c.Symbol)
            .NotEmpty().WithMessage("Symbol must not be empty")
            .MaximumLength(4).WithMessage("Symbol must not be higher than 4 characters");
    }
}