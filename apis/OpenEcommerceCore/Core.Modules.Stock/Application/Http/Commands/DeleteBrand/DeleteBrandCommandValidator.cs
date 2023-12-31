using Core.Modules.Stock.Domain.Contracts.Http.Commands.DeleteBrand;
using FluentValidation;

namespace Core.Modules.Stock.Application.Http.Commands.DeleteBrand;

internal class DeleteBrandCommandValidator : AbstractValidator<DeleteBrandCommand>
{
    public DeleteBrandCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("Id must not be empty");
    }
}