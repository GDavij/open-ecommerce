using Core.Modules.Stock.Domain.Contracts.Http.Queries.ListProductTags;
using FluentValidation;

namespace Core.Modules.Stock.Application.Http.Queries.ListProductTags;

internal class ListProductTagsQueryValidator : AbstractValidator<ListProductTagsQuery>
{
    public ListProductTagsQueryValidator()
    {
        RuleFor(q => q.Page)
            .NotEmpty().WithMessage("Page must not be empty")
            .GreaterThan(0).WithMessage("Page must be greater than 0");
    }
}