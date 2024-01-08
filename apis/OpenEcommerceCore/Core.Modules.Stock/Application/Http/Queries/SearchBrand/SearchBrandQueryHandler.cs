using Core.Modules.Shared.Domain.DataStructures;
using Core.Modules.Shared.Domain.Extensions;
using Core.Modules.Stock.Domain.Contracts.Contexts;
using Core.Modules.Stock.Domain.Contracts.Http.Queries.SearchBrand;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.Stock.Application.Http.Queries.SearchBrand;

internal class SearchBrandQueryHandler : ISearchBrandQueryHandler
{
    private readonly IStockContext _dbContext;
    private const int _numberOfRowsToReturn = 50;

    public SearchBrandQueryHandler(IStockContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PaginatedList<SearchBrandQueryResponse>> Handle(SearchBrandQuery request, CancellationToken cancellationToken)
    {
        var brands = await _dbContext.Brands
            .Select(b => new SearchBrandQueryResponse
            {
                Id = b.Id,
                Name = b.Name,
                Description = b.Description
            })
            .Where(b => EF.Functions.ILike(b.Name + " " + b.Description, $"%{request.SearchTerm}%"))
            .ToPaginatedListAsync(_numberOfRowsToReturn, request.Page, cancellationToken);

        return brands;
    }
}