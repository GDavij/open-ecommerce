using Core.Modules.Shared.Messaging.IntegrationEvents.Stock.Dtos;
using Core.Modules.Stock.Domain.Entities.Product;

namespace Core.Modules.Stock.Domain.DtosMappings;

internal static class ProductToDtoMappings
{
    public static ProductDto MapToProductDto(this Product product)
    {
        return ProductDto.Create(
            id: product.Id,
            brand: BrandDto.Create(
                id: product.Brand.Id,
                description: product.Brand.Description,
                name: product.Brand.Name,
                productsIds: product.Brand.Products.Select(b => b.Id).ToList()),
            name: product.Name,
            description: product.Description,
            price: product.Price,
            stockUnitCount: product.StockUnitCount,
            createdAt: product.CreatedAt,
            lastUpdate: product.LastUpdate,
            tags: product.Tags.Select(t => ProductTagDto.Create(
                id: t.Id,
                name: t.Name,
                taggedProductsIds: t.TaggedProducts.Select(tp => tp.Id).ToList()
            )).ToList(),
            images: product.Images.Select(i => ProductImageDto.Create(
                id: i.Id,
                description: i.Description,
                productId: i.Product.Id,
                url: i.Url
            )).ToList(),
            measurements: product.Measurements.Select(m => ProductDetailDto.Create(
                id: m.Id,
                productId: m.Product.Id,
                showOrder: m.ShowOrder,
                name: m.Name,
                value: m.Value,
                measureUnitDto: m.MeasureUnit is null ? null : MeasureUnitDto.Create(
                        id: m.MeasureUnit.Id,
                        name: m.MeasureUnit.Name,
                        shortName: m.MeasureUnit.ShortName,
                        symbol: m.MeasureUnit.Symbol
                )
            )).ToList(),
            technicalDetails: product.TechnicalDetails.Select(t => ProductDetailDto.Create(
                id: t.Id,
                productId: t.Product.Id,
                showOrder: t.ShowOrder,
                name: t.Name,
                value: t.Value,
                measureUnitDto: t.MeasureUnit is null ? null : MeasureUnitDto.Create(
                    id: t.MeasureUnit.Id,
                    name: t.MeasureUnit.Name,
                    shortName: t.MeasureUnit.ShortName,
                    symbol: t.MeasureUnit.Symbol
                )
            )).ToList(),
            otherDetails: product.OtherDetails.Select(o => ProductDetailDto.Create(
                id: o.Id,
                productId: o.Product.Id,
                showOrder: o.ShowOrder,
                name: o.Name,
                value: o.Value,
                measureUnitDto: o.MeasureUnit is null ? null : MeasureUnitDto.Create(
                    id: o.MeasureUnit.Id,
                    name: o.MeasureUnit.Name,
                    shortName: o.MeasureUnit.ShortName,
                    symbol: o.MeasureUnit.Symbol
                )
            )).ToList()
        );
    }
}