using Core.Modules.HumanResources.Domain.Contracts.Context;
using Core.Modules.HumanResources.Domain.Contracts.Http.Queries.States.ListStates;
using Core.Modules.Shared.Domain.DataStructures;
using Core.Modules.Shared.Domain.Extensions;

namespace Core.Modules.HumanResources.Application.Http.Queries.States.ListStates;

internal class ListStatesQueryHandler : IListStatesQueryHandler
{
    private readonly IHumanResourcesContext _dbContext;
    private const int RowsPerPage = 50; 

    public ListStatesQueryHandler(IHumanResourcesContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PaginatedList<ListStatesQueryResponse>> Handle(ListStatesQuery request, CancellationToken cancellationToken)
    {
        var states = await _dbContext.States
            .OrderBy(s => s.Name)
            .Select(s => new ListStatesQueryResponse(s.Id, s.Name, s.ShortName))
            .ToPaginatedListAsync(RowsPerPage, request.Page, cancellationToken);

        return states;
    }
}