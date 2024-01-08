using Core.Modules.Stock.Domain.Contracts.Http.Queries.GetProductTag;
using FluentValidation;

namespace Core.Modules.Stock.Application.Http.Queries.GetProductTag;

internal class GetProductTagQueryValidator : AbstractValidator<GetProductTagQuery>
{
    public GetProductTagQueryValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty().WithMessage("Id must not be empty");
    }
}