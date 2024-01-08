using Core.Modules.Stock.Domain.Contracts.Http.Queries.GetMeasureUnit;
using FluentValidation;

namespace Core.Modules.Stock.Application.Http.Queries.GetMeasureUnit;

internal class GetMeasureUnitQueryValidator : AbstractValidator<GetMeasureUnitQuery>
{
    public GetMeasureUnitQueryValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty().WithMessage("Id must not be empty");
    }
}