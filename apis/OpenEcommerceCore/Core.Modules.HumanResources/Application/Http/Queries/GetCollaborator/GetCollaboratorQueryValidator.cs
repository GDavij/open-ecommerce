using Core.Modules.HumanResources.Domain.Contracts.Http.Queries;
using FluentValidation;

namespace Core.Modules.HumanResources.Application.Http.Queries.GetCollaborator;

internal class GetCollaboratorQueryValidator : AbstractValidator<GetCollaboratorQuery>
{
    public GetCollaboratorQueryValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("Id must not be empty");
    }
}