using FluentValidation;

namespace Core.Modules.UserAccess.Application.Http.Commands.CreateCollaboratorSession;

internal class CreateCollaboratorSessionCommandValidator : AbstractValidator<CreateCollaboratorSessionCommand>
{
    public CreateCollaboratorSessionCommandValidator()
    {
        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("Email must not be empty")
            .EmailAddress().WithMessage("Email must be a valid one")
            .MaximumLength(255).WithMessage("Email Must have a maximum of 255 characters");

        RuleFor(c => c.Password)
            .NotEmpty().WithMessage("Password must not be empty")
            .MaximumLength(512).WithMessage("Password must have a maximum of 512 characters");
    }
}