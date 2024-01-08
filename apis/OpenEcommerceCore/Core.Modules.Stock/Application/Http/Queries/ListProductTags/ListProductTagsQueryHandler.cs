using Core.Modules.Shared.Domain.DataStructures;
using Core.Modules.Shared.Domain.Extensions;
using Core.Modules.Stock.Domain.Contracts.Contexts;
using Core.Modules.Stock.Domain.Contracts.Http.Queries.ListProductTags;

namespace Core.Modules.Stock.Application.Http.Queries.ListProductTags;

internal class ListProductTagsQueryHandler : IListProductTagsQueryHandler
{
    private const int _rowsPerPage = 50;
    private readonly IStockContext _dbContext;

    public ListProductTagsQueryHandler(IStockContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PaginatedList<ListProductTagsQueryResponse>> Handle(Domain.Contracts.Http.Queries.ListProductTags.ListProductTagsQuery request, CancellationToken cancellationToken)
    {
        var tags = await _dbContext.ProductTags
            .Select(pt => new ListProductTagsQueryResponse
            {
                Id = pt.Id,
                Name = pt.Name
            })
            .ToPaginatedListAsync(_rowsPerPage, request.Page, cancellationToken);

        return tags;
    }
}