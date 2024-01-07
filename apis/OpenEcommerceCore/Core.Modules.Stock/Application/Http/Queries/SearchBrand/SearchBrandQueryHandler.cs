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

    public async Task<SearchBrandQueryResponse> Handle(SearchBrandQuery request, CancellationToken cancellationToken)
    {
        int numberOfRowsToSkip = (request.Page - 1) * _numberOfRowsToReturn;
        
        var brands = await _dbContext.Brands
            .Select(b => new SearchBrandQueryResponse.FoundedBrandSearch 
            {
                Id = b.Id,
                Name = b.Name,
                Description = b.Description
            })
            .Where(b => EF.Functions.ILike(b.Name + " " + b.Description, $"%{request.SearchTerm}%"))
            .Skip(numberOfRowsToSkip)
            .Take(_numberOfRowsToReturn)
            .ToListAsync(cancellationToken);

        return new SearchBrandQueryResponse
        {
            BrandsFound = brands,
            pageIndex = request.Page,
            MaxPages = (brands.Count + numberOfRowsToSkip + _numberOfRowsToReturn - 1) / _numberOfRowsToReturn
        };
    }
}