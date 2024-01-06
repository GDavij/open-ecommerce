using System.Text;
using Core.Modules.Stock.Domain.Contracts.Contexts;
using Core.Modules.Stock.Domain.Contracts.Http.Queries.SearchProduct;
using Core.Modules.Stock.Domain.Entities.Product;
using Core.Modules.Stock.Domain.Exceptions.Product;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.Stock.Application.Http.Queries.SearchProducts;

internal class SearchProductQueryHandler : ISearchProductQueryHandler
{
    private readonly IStockContext _dbContext;

    private const int _numberOfRowsToReturn = 50;

    public SearchProductQueryHandler(IStockContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<SearchProductQueryResponse> Handle(SearchProductQuery request,
        CancellationToken cancellationToken)
    {
        int numberOfRowsToSkip = (request.Page - 1) * _numberOfRowsToReturn;

        var queryable = _dbContext.Products
            .Include(p => p.Brand)
            .Include(p => p.Tags)
            .AsQueryable();

        if (request.TagsIds.Count > 0)
        {
            var validTags = await _dbContext.ProductTags.Select(pt => pt.Id).ToListAsync(cancellationToken);
            var validRequestTags = request.TagsIds.Where(ti => validTags.Contains(ti)).ToList();
            if (validRequestTags.Count < request.TagsIds.Count)
            {
                var firstInvalidTag = request.TagsIds.First(ti => !validTags.Contains(ti));
                throw new InvalidProductTagException(firstInvalidTag);
            }

            queryable = queryable.Where(p => p.Tags.Any(t => validTags.Contains(t.Id)));
        }

        if (request.BrandsIds.Count > 0)
        {
            var validBrands = await _dbContext.Brands.Select(b => b.Id).ToListAsync(cancellationToken);
            var validRequestBrands = request.BrandsIds.Where(bi => validBrands.Contains(bi)).ToList();
            if (validBrands.Count < request.BrandsIds.Count)
            {
                var firstInvalidBrand = request.BrandsIds.First(bi => !validBrands.Contains(bi));
                throw new InvalidBrandException(firstInvalidBrand);
            }

            queryable = queryable.Where(p => validRequestBrands.Contains(p.Brand.Id));
        }
        
        var products = await queryable
                .Skip(numberOfRowsToSkip)
                .Take(_numberOfRowsToReturn)
                .ToListAsync(cancellationToken); 
        
        return new SearchProductQueryResponse
        {
            ProductsFound = products.Select(p => new FoundedProductSearch
            {
                ProductId = p.Id,
                BrandName = p.Brand.Name,
                Name = p.Name,
                Description = p.Description,
                Sku = p.SKU,
                Ean = p.EAN,
                Upc = p.UPC,
                Price = p.Price,
                Tags = p.Tags.Select(t => new ProductTagResponse
                {
                    Id = t.Id,
                    Name = t.Name
                }).ToList(),
                StockUnitCount = p.StockUnitCount,
                CreatedAt = p.CreatedAt,
                LastUpdate = p.LastUpdate
            }).ToList(),
            pageIndex = request.Page,
            MaxPages = (products.Count + numberOfRowsToSkip + _numberOfRowsToReturn - 1) / _numberOfRowsToReturn
        };
    }
}