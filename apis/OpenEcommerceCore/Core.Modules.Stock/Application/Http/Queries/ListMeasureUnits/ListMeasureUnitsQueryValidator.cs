using Core.Modules.Stock.Domain.Contracts.Http.Queries.ListMeasureUnits;
using FluentValidation;

namespace Core.Modules.Stock.Application.Http.Queries.ListMeasureUnits;

internal class ListMeasureUnitsQueryValidator : AbstractValidator<ListMeasureUnitsQuery>
{
    public ListMeasureUnitsQueryValidator()
    {
        RuleFor(q => q.Page)
            .NotEmpty().WithMessage("Page must not be empty")
            .GreaterThan(0).WithMessage("Page must be greater than 0");
    }
}