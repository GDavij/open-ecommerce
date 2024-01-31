using Core.Modules.HumanResources.Domain.Contracts.Http.Queries.SearchCollaborators;
using FluentValidation;

namespace Core.Modules.HumanResources.Application.Http.Queries.SearchCollaborators;

internal class SearchCollaboratorsQueryValidator : AbstractValidator<SearchCollaboratorsQuery>
{
    public SearchCollaboratorsQueryValidator()
    {
        RuleFor(q => q.SearchTerm)
            .MaximumLength(255).WithMessage("Search Term must not be greater than 255 characters");

        RuleFor(q => q.Sector)
            .IsInEnum().When(q => q.Sector is not null).WithMessage("Enum must be a valida one");
    }
}