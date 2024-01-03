using Core.Modules.Stock.Domain.Contracts.Http.Queries.GetProduct;
using FluentValidation;

namespace Core.Modules.Stock.Application.Http.Queries.GetProduct;

internal class GetProductQueryValidator : AbstractValidator<GetProductQuery>
{
    public GetProductQueryValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("Id must not be empty");
    }
}