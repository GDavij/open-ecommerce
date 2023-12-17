using Core.Modules.Stock.Domain.Contracts.Contexts;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.CreateProduct;
using Core.Modules.Stock.Domain.Contracts.Providers;
using Core.Modules.Stock.Domain.Entities.Product;
using Core.Modules.Stock.Domain.Entities.Product.ProductDetails;
using Core.Modules.Stock.Domain.Exceptions.Product;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.Stock.Application.Http.Commands.CreateProductCommand;

internal class CreateProductCommandHandler : ICreateProductCommandHandler
{
    private readonly IStockContext _context;
    private readonly IStockDateTimeProvider _stockDateTimeProvider;

    public CreateProductCommandHandler(IStockContext context, IStockDateTimeProvider stockDateTimeProvider)
    {
        _context = context;
        _stockDateTimeProvider = stockDateTimeProvider;
    }

    public async Task Handle(Domain.Contracts.Http.Commands.CreateProduct.CreateProductCommand request, CancellationToken cancellationToken)
    {
        // If Possible Move Validation Logic to the validation middleware of command
        if (!(await _context.Brands.AnyAsync(b => b.Id == request.BrandId, cancellationToken)))
        {
            throw new InvalidBrandException(request.BrandId);
        }

        if (await _context.Products.AnyAsync(p => p.EAN == request.Ean, cancellationToken))
        {
            throw new ExistentEanCodeException(request.Ean);
        }

        if (request.Upc is not null && 
            await _context.Products.AnyAsync(p => p.UPC == request.Upc, cancellationToken))
        {
            throw new ExistentUpcCodeException(request.Upc);
        }

        if (request.Sku is not null && 
            await _context.Products.AnyAsync(p => p.SKU == request.Sku, cancellationToken))
        {
            throw new ExistentSkuCodeException(request.Sku);
        }
        
        List<ProductTag> validTags = await _context.ProductTags
            .Where(pt => request.TagsIds.Contains(pt.Id))
            .ToListAsync(cancellationToken);

        if (validTags.Count < request.TagsIds.Count)
        {
            var validTagsIds = validTags.Select(v => v.Id);

            Guid firstInvalidTagId = request.TagsIds
                .First(ti => !validTagsIds.Contains(ti));

            throw new InvalidProductTagException(firstInvalidTagId);
        }

        List<Guid> measureUnitIds = request.Measurements
            .Select(m => m.MeasureUnitId.GetValueOrDefault())
            .Concat(request.TechinicalDetails.Select(t => t.MeasureUnitId.GetValueOrDefault()))
            .Concat(request.OtherDetails.Select(o => o.MeasureUnitId.GetValueOrDefault()))
            .Where(ids => ids != Guid.Empty)
            .Distinct()
            .ToList();

        var validMeasureUnits = await _context.MeasureUnits
            .Where(m => measureUnitIds.Contains(m.Id))
            .ToListAsync(cancellationToken);
 
        if (validMeasureUnits.Count < measureUnitIds.Count)
        {
            var validMeasureUnitIds = validMeasureUnits.Select(v => v.Id);

            Guid firstInvalidMeasureUnitId = measureUnitIds
                .First(m => !validMeasureUnitIds.Contains(m));
            
            throw new InvalidMeasureUnitException(firstInvalidMeasureUnitId);
        }
        

        var brand = await _context.Brands
            .FirstAsync(b => b.Id == request.BrandId, cancellationToken);
        
        var product = Product.Create(
            brand: brand,
            name: request.Name,
            description: request.Description,
            sku: request.Sku,
            ean: request.Ean,
            upc: request.Upc,
            price: request.Price,
            stockUnitCount: request.StockUnitCount,
            stockDateTimeProvider: _stockDateTimeProvider);

        product.Tags = validTags;
        
        product.Measurements = request.Measurements
            .Select(m => MeasurementDetail.Create(
                product: product,
                showOrder: m.ShowOrder,
                name: m.Name,
                value: m.Value,
                measureUnit: validMeasureUnits.FirstOrDefault(v => v.Id == m.MeasureUnitId)))
            .ToList();
        
        product.TechnicalDetails = request.TechinicalDetails
            .Select(t => TechnicalDetail.Create(
                product: product,
                showOrder: t.ShowOrder,
                name: t.Name,
                value: t.Value,
                measureUnit: validMeasureUnits.FirstOrDefault(v => v.Id == t.MeasureUnitId)))
            .ToList();
        
        product.OtherDetails = request.OtherDetails
            .Select(o => OtherDetail.Create(
                product: product,
                showOrder: o.ShowOrder,
                name: o.Name,
                value: o.Value,
                measureUnit: validMeasureUnits.FirstOrDefault(v => v.Id == o.MeasureUnitId)))
            .ToList();
        
        _context.Products.Add(product);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
