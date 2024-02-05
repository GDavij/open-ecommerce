using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Commands.Contracts.RemoveWorkHourFromContributionYear;

public record RemoveWorkHourFromContributionYearCommand(Guid Id) : IRequest;