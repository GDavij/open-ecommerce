using Core.Modules.HumanResources.Domain.Contracts.Context;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.Contracts.AddWorkHourToContributionYear;
using Core.Modules.HumanResources.Domain.Entities;
using Core.Modules.HumanResources.Domain.Exceptions.Contracts;
using Core.Modules.HumanResources.Domain.Exceptions.ContributionYear;
using Core.Modules.HumanResources.Domain.Extensions;
using Core.Modules.Shared.Domain.Contracts.Services;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.HumanResources.Application.Http.Commands.Contracts.AddWorkHourToContributionYear;

internal class AddWorkHourToContributionYearCommandHandler : IAddWorkHourToContributionYearCommandHandler
{
    private readonly IHumanResourcesContext _dbContext;
    private readonly IAppConfigService _configService;

    public AddWorkHourToContributionYearCommandHandler(IHumanResourcesContext dbContext, IAppConfigService configService)
    {
        _dbContext = dbContext;
        _configService = configService;
    }
    
    public async Task<AddWorkHourToContributionYearCommandResponse> Handle(AddWorkHourToContributionYearCommand request, CancellationToken cancellationToken)
    {
        var contract = await _dbContext.Contracts
            .Include(x => x.ContributionYears)
                .ThenInclude(x => x.WorkHours)
            .WhereValidContracts()
            .FirstOrDefaultAsync(x => x.Id == request.ContractId, cancellationToken);

        if (contract is null)
        {
            throw new InvalidContractException(request.ContractId);
        }

        var workHourYear = request.WorkHour.Date.Year;
        var contributionYear = contract.ContributionYears.FirstOrDefault(x => x.Year == workHourYear);
        if (contributionYear is null)
        {
            throw new InvalidYearOfContributionException(workHourYear);
        }

        if (contributionYear.WorkHours.Any(x => x.Date == request.WorkHour.Date))
        {
            throw new AlreadyRegisteredWorkHourException(request.WorkHour.Date);
        }
        
        var workHour = new WorkHour
        {
            Id = Guid.NewGuid(),
            Date = request.WorkHour.Date,
            Start = request.WorkHour.Start,
            End = request.WorkHour.End
        };

        contributionYear.WorkHours.Add(workHour);

        _dbContext.Entry(workHour).State = EntityState.Added;
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        return AddWorkHourToContributionYearCommandResponse.Respond(contract.Id, _configService);
    }
}