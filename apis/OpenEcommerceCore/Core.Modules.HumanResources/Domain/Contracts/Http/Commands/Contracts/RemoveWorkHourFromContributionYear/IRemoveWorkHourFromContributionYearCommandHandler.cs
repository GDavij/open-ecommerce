using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Commands.Contracts.RemoveWorkHourFromContributionYear;

internal interface IRemoveWorkHourFromContributionYearCommandHandler 
    : IRequestHandler<RemoveWorkHourFromContributionYearCommand>
{ }