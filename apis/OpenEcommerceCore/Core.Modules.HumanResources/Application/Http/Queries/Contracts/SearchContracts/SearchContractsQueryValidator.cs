using Core.Modules.HumanResources.Domain.Contracts.Http.Queries.Contracts.SearchContracts;
using FluentValidation;

namespace Core.Modules.HumanResources.Application.Http.Queries.Contracts.SearchContracts;

internal class SearchContractsQueryValidator : AbstractValidator<SearchContractsQuery>
{
    public SearchContractsQueryValidator()
    {
        RuleFor(x => x.SearchTerm)
            .MaximumLength(255).WithMessage("Search term cannot be greater than 255 characters");

        RuleFor(x => x.Sector)
            .IsInEnum().When(x => x.Sector is not null).WithMessage("Sector must be valid");

        RuleFor(x => x.FromDate)
            .Must((ctx, fromDateTime) => fromDateTime <= ctx.TillDate).When(x => x.TillDate is not null && x.FromDate is not null).WithMessage("From Date must not be greater than Till Date");
       
        RuleFor(x => x.TillDate)
            .Must((ctx, tillDate) => tillDate >= ctx.FromDate).When(x => x.TillDate is not null && x.FromDate is not null).WithMessage("Till Date must not be lower than From Date");

        RuleFor(x => x.Page)
            .NotEmpty().WithMessage("Page must not be empty")
            .GreaterThan(0).WithMessage("Page must be greater than 0");
    }
}