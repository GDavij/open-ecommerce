using Core.Modules.Stock.Domain.Contracts.Http.Commands.UpdateBrand;
using FluentValidation;

namespace Core.Modules.Stock.Application.Http.Commands.DeleteBrand;

internal class UpdateBrandCommandValidator : AbstractValidator<UpdateBrandCommand>
{
    public UpdateBrandCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("Id must not be empty");

        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Name must not be empty")
            .MaximumLength(255).WithMessage("Name must not be greater than 255 characters");

        RuleFor(c => c.Description)
            .MaximumLength(512).When(c => !string.IsNullOrEmpty(c.Description)).WithMessage("Description must not be greater than 512 characters");
    }
}