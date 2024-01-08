using Core.Modules.Stock.Domain.Contracts.Http.Commands.DeleteProductTag;
using FluentValidation;

namespace Core.Modules.Stock.Application.Http.Commands.DeleteProductTag;

internal class DeleteProductTagCommandValidator : AbstractValidator<DeleteProductTagCommand>
{
    public DeleteProductTagCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("Id must not be empty");
    }
}