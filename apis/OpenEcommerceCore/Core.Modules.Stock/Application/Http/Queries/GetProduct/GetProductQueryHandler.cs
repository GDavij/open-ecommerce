using Core.Modules.Stock.Domain.Contracts.Contexts;
using Core.Modules.Stock.Domain.Contracts.Http.Queries.GetProduct;
using Core.Modules.Stock.Domain.Exceptions.Product;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.Stock.Application.Http.Queries.GetProduct;

internal class GetProductQueryHandler : IGetProductQueryHandler
{
    private readonly IStockContext _dbContext;

    public GetProductQueryHandler(IStockContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<GetProductQueryResponse> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (product is null)
        {
            throw new InvalidProductException(request.Id);
        }

        return new GetProductQueryResponse
        {
            Id = product.Id,
            BrandId = product.Brand.Id,
            Name = product.Name,
            Description = product.Description,
            Sku = product.SKU,
            Ean = product.EAN,
            Upc = product.UPC,
            Price = product.Price,
            StockUnitCount = product.StockUnitCount,
            CreatedAt = product.CreatedAt,
            LastUpdate = product.LastUpdate,
            ProductSuppliers = product.Suppliers.Select(s => s.Id).ToList(),
            ProductTags = product.Tags.Select(pt => new GetProductQueryResponse.TagResponse
            {
                Id = pt.Id,
                Name = pt.Name
            }).ToList(),
            ProductImages = product.Images.Select(pi => new GetProductQueryResponse.ImageResponse
            {
                Id = pi.Id,
                Description = pi.Description,
                Url = pi.Url
            }).ToList(),
            Measurements = product.Measurements.Select(pmd => new GetProductQueryResponse.ProductDetailResponse
            {
                Id = pmd.Id,
                Name = pmd.Name,
                ShowOrder = pmd.ShowOrder,
                Value = pmd.Value,
                MeasureUnit = pmd.MeasureUnit is not null ? new GetProductQueryResponse.MeasureUnitResponse
                {
                    Id = pmd.MeasureUnit.Id,
                    Name = pmd.MeasureUnit.Name,
                    ShortName = pmd.MeasureUnit.ShortName,
                    Symbol = pmd.MeasureUnit.Symbol
                } : null
            }).ToList(),
            TechnicalDetails = product.TechnicalDetails.Select(ptd =>
                new GetProductQueryResponse.ProductDetailResponse
                {
                    Id = ptd.Id,
                    Name = ptd.Name,
                    ShowOrder = ptd.ShowOrder,
                    Value = ptd.Value,
                    MeasureUnit = ptd.MeasureUnit is not null ? new GetProductQueryResponse.MeasureUnitResponse
                    {
                        Id = ptd.MeasureUnit.Id,
                        Name = ptd.MeasureUnit.Name,
                        ShortName = ptd.MeasureUnit.ShortName,
                        Symbol = ptd.MeasureUnit.Symbol
                    } : null
                }).ToList(),
            OtherDetails = product.OtherDetails.Select(pod => new GetProductQueryResponse.ProductDetailResponse
            {
                Id = pod.Id,
                Name = pod.Name,
                ShowOrder = pod.ShowOrder,
                Value = pod.Value,
                MeasureUnit = pod.MeasureUnit is not null ? new GetProductQueryResponse.MeasureUnitResponse
                {
                    Id = pod.MeasureUnit.Id,
                    Name = pod.MeasureUnit.Name,
                    ShortName = pod.MeasureUnit.ShortName,
                    Symbol = pod.MeasureUnit.Symbol
                } : null
            }).ToList()
        };
    }
}