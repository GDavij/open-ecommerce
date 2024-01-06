using Core.Modules.Stock.Domain.Contracts.Http.Queries.SearchBrand;
using FluentValidation;

namespace Core.Modules.Stock.Application.Http.Queries.SearchBrand;

internal class SearchBrandQueryValidator : AbstractValidator<SearchBrandQuery>
{
    public SearchBrandQueryValidator()
    {
        RuleFor(q => q.Page)
            .NotEmpty().WithMessage("Page must not be empty")
            .GreaterThan(0).WithMessage("Page must be greater than 0");

        RuleFor(q => q.SearchTerm)
            .MaximumLength(512).When(q => !string.IsNullOrEmpty(q.SearchTerm)).WithMessage("Search Term must not be higher than 512 characters");
    }
}