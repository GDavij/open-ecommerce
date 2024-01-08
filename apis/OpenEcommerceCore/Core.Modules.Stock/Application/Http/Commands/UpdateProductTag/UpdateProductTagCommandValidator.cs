using Core.Modules.Stock.Domain.Contracts.Http.Commands.UpdateProductTag;
using FluentValidation;

namespace Core.Modules.Stock.Application.Http.Commands.UpdateProductTag;

internal class UpdateProductTagCommandValidator : AbstractValidator<UpdateProductTagCommand>
{
    public UpdateProductTagCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("Id must not be empty");

        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Name must not be empty")
            .MaximumLength(128).WithMessage("Name must not be greater than 128 characters");
    }
}