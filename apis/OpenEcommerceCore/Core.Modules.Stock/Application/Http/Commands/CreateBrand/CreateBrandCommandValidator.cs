using Core.Modules.Stock.Domain.Contracts.Http.Commands.CreateBrand;
using FluentValidation;

namespace Core.Modules.Stock.Application.Http.Commands.CreateBrand;

internal class CreateBrandCommandValidator : AbstractValidator<CreateBrandCommand>
{
    public CreateBrandCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Name must not be empty")
            .MaximumLength(255).WithMessage("Name must not be greater than 255 characters");

        RuleFor(c => c.Description)
            .MaximumLength(512).When(c => c.Description is not null).WithMessage("Description must not be greater than 512 characters");
    }
}