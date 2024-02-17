using Core.Modules.HumanResources.Domain.Contracts.Http.Queries.States.ListStates;
using FluentValidation;

namespace Core.Modules.HumanResources.Application.Http.Queries.States.ListStates;

internal class ListStatesQueryValidator : AbstractValidator<ListStatesQuery>
{
    public ListStatesQueryValidator()
    {
        RuleFor(q => q.Page)
            .GreaterThan(0).WithMessage("Page must be greater than 0");
    }
}