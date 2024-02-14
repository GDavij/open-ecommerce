using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.States.CreateState;
using FluentValidation;

namespace Core.Modules.HumanResources.Application.Http.Commands.States.CreateState;

internal class CreateStateCommandValidator : AbstractValidator<CreateStateCommand>
{
    public CreateStateCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name must not be empty")
            .MaximumLength(128).WithMessage("Name must not be greater than 128 characters");
        
        RuleFor(x => x.ShortName)
            .NotEmpty().WithMessage("ShortName must not be empty")
            .MaximumLength(4).WithMessage("Short Name must not be greater than 4 characters");
    }
}