using Core.Modules.Stock.Domain.Contracts.Http.Queries.GetBrand;
using FluentValidation;

namespace Core.Modules.Stock.Application.Http.Queries.GetBrand;

internal class GetBrandQueryValidator : AbstractValidator<GetBrandQuery>
{
    public GetBrandQueryValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty().WithMessage("Id must not be empty");
    }
}