using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Commands.Contracts.AddWorkHourToContributionYear;

internal interface IAddWorkHourToContributionYearCommandHandler
    : IRequestHandler<AddWorkHourToContributionYearCommand, AddWorkHourToContributionYearCommandResponse>
{ }