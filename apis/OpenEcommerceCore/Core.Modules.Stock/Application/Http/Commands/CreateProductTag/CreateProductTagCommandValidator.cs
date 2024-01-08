using Core.Modules.Stock.Domain.Contracts.Http.Commands.CreateProductTag;
using FluentValidation;

namespace Core.Modules.Stock.Application.Http.Commands.CreateProductTag;

internal class CreateProductTagCommandValidator : AbstractValidator<CreateProductTagCommand>
{
    public CreateProductTagCommandValidator()
    {
        RuleFor(q => q.Name)
            .NotEmpty().WithMessage("Name must not be empty")
            .MaximumLength(128).WithMessage("Name must not be greater than 128 characters");
    }
}