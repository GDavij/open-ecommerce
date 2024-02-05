using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Commands.Contracts.AddWorkHourToContributionYear;

public record AddWorkHourToContributionYearCommand : IRequest<AddWorkHourToContributionYearCommandResponse>
{
    public Guid ContractId { get; init; }
    public WorkHourRequestSchema WorkHour { get; init; }

    public record WorkHourRequestSchema
    {
        public DateTime Date { get; init; }
        public TimeSpan Start { get; init; }
        public TimeSpan End { get; init; }
    }
}