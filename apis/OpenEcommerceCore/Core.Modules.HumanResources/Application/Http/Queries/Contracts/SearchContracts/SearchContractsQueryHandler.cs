using Core.Modules.HumanResources.Domain.Contracts.Context;
using Core.Modules.HumanResources.Domain.Contracts.Http.Queries.Contracts.SearchContracts;
using Core.Modules.Shared.Domain.DataStructures;
using Core.Modules.Shared.Domain.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.HumanResources.Application.Http.Queries.Contracts.SearchContracts;

internal class SearchContractsQueryHandler : ISearchContractsQueryHandler
{
    private const int RowsPerPage = 50;
    private readonly IHumanResourcesContext _dbContext;

    public SearchContractsQueryHandler(IHumanResourcesContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PaginatedList<SearchContractsQueryResponse>> Handle(SearchContractsQuery request, CancellationToken cancellationToken)
    {
        var contractsQueryable = _dbContext.Contracts
            .Include(x => x.Collaborator)
            .Where(x => EF.Functions.ILike("%" + x.Name + "%" + x.Collaborator.FirstName + "%" + x.Collaborator.LastName + "%", $"%{request.SearchTerm.Replace(' ', '%')}%"));

        var shouldFilterSector = request.Sector != null;
        if (shouldFilterSector)
        {
            contractsQueryable = contractsQueryable.Where(x => x.Sector == request.Sector);
        }
        
        var shouldFilterStartDate = request.FromDate != null;
        if (shouldFilterStartDate)
        {
            contractsQueryable = contractsQueryable.Where(x => x.StartDate >= request.FromDate);
        }

        var shouldFilterEndDate = request.TillDate != null;
        if (shouldFilterEndDate)
        {
            contractsQueryable = contractsQueryable.Where(x => x.EndDate <= request.TillDate);
        }

        var shouldFilterIsBroken = request.IsBroken != null;
        if (shouldFilterIsBroken)
        {
            contractsQueryable = contractsQueryable.Where(x => x.Broken == request.IsBroken);
        }

        var shouldFilterIsDeleted = request.IsDeleted != null;
        if (shouldFilterIsDeleted)
        {
            contractsQueryable = contractsQueryable.Where(x => x.Deleted == request.IsDeleted);
        }
        
        var shouldFilterIsExpired = request.IsExpired != null;
        if (shouldFilterIsExpired)
        {
            contractsQueryable = contractsQueryable.Where(x => (DateTime.UtcNow  > x.EndDate) == request.IsExpired);
        }

        return await contractsQueryable
            .Select(x => new SearchContractsQueryResponse
            {
                Id = x.Id,
                CollaboratorId = x.CollaboratorId,
                CollaboratorName = $"{x.Collaborator.FirstName} {x.Collaborator.LastName}",
                Name = x.Name,
                Sector = x.Sector,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Expired = x.Expired,
                Broken = x.Broken,
                Deleted = x.Deleted
            })
            .ToPaginatedListAsync(RowsPerPage, request.Page, cancellationToken);
    }
}