using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.Collaborators.DeleteCollaborator;
using FluentValidation;

namespace Core.Modules.HumanResources.Application.Http.Commands.Collaborators.DeleteCollaborator;

internal class DeleteCollaboratorCommandValidator : AbstractValidator<DeleteCollaboratorCommand>
{
    public DeleteCollaboratorCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("Id must not be empty");
    }
}