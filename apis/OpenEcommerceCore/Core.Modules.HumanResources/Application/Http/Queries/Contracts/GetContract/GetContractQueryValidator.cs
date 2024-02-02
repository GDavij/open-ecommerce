using Core.Modules.HumanResources.Domain.Contracts.Http.Queries.Contracts.GetContract;
using FluentValidation;

namespace Core.Modules.HumanResources.Application.Http.Queries.Contracts.GetContract;

internal class GetContractQueryValidator : AbstractValidator<GetContractQuery>
{
   public GetContractQueryValidator()
   {
      RuleFor(x => x.Id)
         .NotEmpty().WithMessage("Id must not be empty");
   }
}