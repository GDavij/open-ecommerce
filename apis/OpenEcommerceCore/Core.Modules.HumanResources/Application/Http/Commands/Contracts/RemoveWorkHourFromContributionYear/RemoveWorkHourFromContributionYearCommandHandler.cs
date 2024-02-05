using Core.Modules.HumanResources.Domain.Contracts.Context;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.Contracts.RemoveWorkHourFromContributionYear;
using Core.Modules.HumanResources.Domain.Exceptions.WorkHour;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.HumanResources.Application.Http.Commands.Contracts.RemoveWorkHourFromContributionYear;

internal class RemoveWorkHourFromContributionYearCommandHandler : IRemoveWorkHourFromContributionYearCommandHandler
{
    private readonly IHumanResourcesContext _dbContext;

    public RemoveWorkHourFromContributionYearCommandHandler(IHumanResourcesContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(RemoveWorkHourFromContributionYearCommand request, CancellationToken cancellationToken)
    {
        var workHour = await _dbContext.WorkHours
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (workHour is null)
        {
            throw new InvalidContributionYearException(request.Id);
        }

        _dbContext.WorkHours.Remove(workHour);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}