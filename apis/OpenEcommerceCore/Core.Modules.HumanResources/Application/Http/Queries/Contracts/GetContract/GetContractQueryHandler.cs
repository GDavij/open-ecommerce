using Core.Modules.HumanResources.Domain.Contracts.Context;
using Core.Modules.HumanResources.Domain.Contracts.Http.Queries.Contracts.GetContract;
using Core.Modules.HumanResources.Domain.Exceptions.Contracts;
using Core.Modules.HumanResources.Domain.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.HumanResources.Application.Http.Queries.Contracts.GetContract;

internal class GetContractQueryHandler : IGetContractQueryHandler
{
    private const int SimilarCollaboratorContractsCount = 5;
    private readonly IHumanResourcesContext _dbContext;

    public GetContractQueryHandler(IHumanResourcesContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetContractQueryResponse> Handle(GetContractQuery request, CancellationToken cancellationToken)
    {
        var contract = await _dbContext.Contracts
            .Include(x => x.Collaborator)
                .ThenInclude(x => x.Contracts)
            .Include(x => x.ContributionYears)
                .ThenInclude(x => x.WorkHours)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (contract is null)
        {
            throw new InvalidContractException(request.Id);
        }

        var suggestedActiveCollaboratorContracts = contract.Collaborator.Contracts
            .WhereValidContracts()
            .Take(SimilarCollaboratorContractsCount)
            .Select(x => new GetContractQueryResponse.SuggestedCollaboratorContracts
            {
                Id = x.Id,
                Name = x.Name,
                Sector = x.Sector
            })
            .ToList();
        
        return new GetContractQueryResponse
        {
            Id = contract.Id,
            CollaboratorId = contract.Collaborator.Id,
            Name = contract.Name,
            Sector = contract.Sector,
            SimilarCollaboratorContracts = suggestedActiveCollaboratorContracts,
            ContributionYears = contract.ContributionYears.Select(cy => new GetContractQueryResponse.ContributionYear
            {
                Id = cy.Id,
                Year = cy.Year,
                WorkHours = cy.WorkHours.Select(w => new GetContractQueryResponse.ContributionYear.WorkHour
                {
                    Id = w.Id,
                    Date = w.Date,
                    Start = w.Start,
                    End = w.End,
                    Duration = w.Duration
                }).ToList()
            }).ToList(),
            StartDate = contract.StartDate,
            EndDate = contract.EndDate,
            MonthlySalary = contract.MonthlySalary,
            Expired = contract.Expired,
            Broken = contract.Broken,
            Deleted = contract.Deleted
        };
    }
}