using Core.Modules.Stock.Domain.Contracts.Contexts;
using Core.Modules.Stock.Domain.Contracts.Http.Queries.GetBrand;
using Core.Modules.Stock.Domain.Contracts.Http.Queries.GetProduct;
using Core.Modules.Stock.Domain.Exceptions.Product;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.Stock.Application.Http.Queries.GetBrand;

internal class GetBrandQueryHandler : IGetBrandQueryHandler
{
    private readonly IStockContext _dbContext;

    public GetBrandQueryHandler(IStockContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<GetBrandQueryResponse> Handle(GetBrandQuery request, CancellationToken cancellationToken)
    {
        var brand = await _dbContext.Brands
            .Include(b => b.Products)
                .ThenInclude(p => p.Tags)
            .Select(b => new GetBrandQueryResponse
            {
                Id = b.Id,
                Name = b.Name,
                Description = b.Description,
                Products = b.Products.Select(p => new GetBrandQueryResponse.ProductResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    StockUnitCount = p.StockUnitCount,
                    Tags = p.Tags.Select(t => new GetBrandQueryResponse.TagResponse
                    {
                        Id = t.Id,
                        Name = t.Name
                    }).ToList()
                }).ToList()
            })
            .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

        if (brand is null)
        {
            throw new InvalidBrandException(request.Id);
        }

        return brand;
    }
}