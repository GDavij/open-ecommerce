using Core.Modules.Stock.Domain.Contracts.Http.Commands.RemoveImageFromProduct;
using FluentValidation;

namespace Core.Modules.Stock.Application.Http.Commands.RemoveImageFromProduct;

internal class RemoveImageFromProductCommandValidator : AbstractValidator<RemoveImageFromProductCommand>
{
    public RemoveImageFromProductCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("Product Image must not be empty");
    }
}