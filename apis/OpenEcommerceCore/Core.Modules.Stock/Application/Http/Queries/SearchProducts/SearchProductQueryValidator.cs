using Core.Modules.Stock.Domain.Contracts.Http.Queries.SearchProduct;
using FluentValidation;

namespace Core.Modules.Stock.Application.Http.Queries.SearchProducts;

internal class SearchProductQueryValidator : AbstractValidator<SearchProductQuery>
{
    public SearchProductQueryValidator()
    {
        RuleFor(q => q.SearchTerm)
            .MaximumLength(512).When(q => !string.IsNullOrEmpty(q.SearchTerm)).WithMessage("Search must have at least 512 characters");

        RuleFor(q => q.Page)
            .GreaterThan(0).WithMessage("Page must be greater than 0");

        RuleFor(q => q.BrandsIds)
            .NotNull().WithMessage("Brands Ids must not be null");

        RuleFor(q => q.TagsIds)
            .NotNull().WithMessage("Tags Ids must not be null");
    }
}