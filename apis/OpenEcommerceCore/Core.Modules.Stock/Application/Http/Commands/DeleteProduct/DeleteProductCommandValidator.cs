using Core.Modules.Stock.Domain.Contracts.Http.Commands.DeleteProduct;
using FluentValidation;

namespace Core.Modules.Stock.Application.Http.Commands.DeleteProduct;

internal class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("Id must not be Empty");
    }
}