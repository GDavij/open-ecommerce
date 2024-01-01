using Core.Modules.Shared.Domain.Contracts.Services;
using Core.Modules.Stock.Domain.Contracts.Contexts;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.CreateProduct;
using Core.Modules.Stock.Domain.Contracts.Providers;
using Core.Modules.Stock.Domain.Entities.Product;
using Core.Modules.Stock.Domain.Entities.Product.ProductDetails;
using Core.Modules.Stock.Domain.Exceptions.Product;
using Core.Modules.Stock.Domain.IntegrationEvents.Models.Mappings;
using Core.Modules.Stock.Domain.IntegrationEvents.Product;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.Stock.Application.Http.Commands.CreateProduct;

internal class CreateProductCommandHandler : ICreateProductCommandHandler
{
    private readonly IStockContext _dbContext;
    private readonly IStockDateTimeProvider _stockDateTimeProvider;
    private readonly IAppConfigService _configService;
    private readonly IPublishEndpoint _publishEndpoint;
    public CreateProductCommandHandler(
        IStockContext dbContext,
        IStockDateTimeProvider stockDateTimeProvider,
        IPublishEndpoint publishEndpoint,
        IAppConfigService configService)
    {
        _dbContext = dbContext;
        _stockDateTimeProvider = stockDateTimeProvider;
        _publishEndpoint = publishEndpoint;
        _configService = configService;
    }

    public async Task<CreateProductCommandResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // If Possible Move Validation Logic to the validation middleware of command
        if (!(await _dbContext.Brands.AnyAsync(b => b.Id == request.BrandId, cancellationToken)))
        {
            throw new InvalidBrandException(request.BrandId);
        }

        //Is Product Name Valid
        if (await _dbContext.Products.AnyAsync(p => p.Name == request.Name, cancellationToken))
        {
            throw new ExistentProductNameException(request.Name);
        }

        if (await _dbContext.Products.AnyAsync(p => p.EAN == request.Ean, cancellationToken))
        {
            throw new ExistentEanCodeException(request.Ean);
        }

        if (request.Upc is not null &&
            await _dbContext.Products.AnyAsync(p => p.UPC == request.Upc, cancellationToken))
        {
            throw new ExistentUpcCodeException(request.Upc);
        }

        if (request.Sku is not null &&
            await _dbContext.Products.AnyAsync(p => p.SKU == request.Sku, cancellationToken))
        {
            throw new ExistentSkuCodeException(request.Sku);
        }

        bool existsRepeatedShowOrderInMeasurements = request.Measurements
            .GroupBy(m => m.ShowOrder)
            .Count(m => m.Count() > 1) > 0;

        if (existsRepeatedShowOrderInMeasurements)
        {
            throw new ShowOrderRepeatedException(ShowOrderRepeatedEncountered.Measures);
        }

        bool existsRepeatedShowOrderInTechnicalDetails = request.TechnicalDetails
            .GroupBy(t => t.ShowOrder)
            .Count(t => t.Count() > 1) > 0;

        if (existsRepeatedShowOrderInTechnicalDetails)
        {
            throw new ShowOrderRepeatedException(ShowOrderRepeatedEncountered.TechnicalDetails);
        }

        bool existsRepeatedShowOrderInOtherDetails = request.OtherDetails
            .GroupBy(o => o.ShowOrder)
            .Count(o => o.Count() > 1) > 0;

        if (existsRepeatedShowOrderInOtherDetails)
        {
            throw new ShowOrderRepeatedException(ShowOrderRepeatedEncountered.OtherDetails);
        }

        List<ProductTag> validTags = await _dbContext.ProductTags
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
            .Concat(request.TechnicalDetails.Select(t => t.MeasureUnitId.GetValueOrDefault()))
            .Concat(request.OtherDetails.Select(o => o.MeasureUnitId.GetValueOrDefault()))
            .Where(ids => ids != Guid.Empty)
            .Distinct()
            .ToList();

        var validMeasureUnits = await _dbContext.MeasureUnits
            .Where(m => measureUnitIds.Contains(m.Id))
            .ToListAsync(cancellationToken);

        if (validMeasureUnits.Count < measureUnitIds.Count)
        {
            var validMeasureUnitIds = validMeasureUnits.Select(v => v.Id);

            Guid firstInvalidMeasureUnitId = measureUnitIds
                .First(m => !validMeasureUnitIds.Contains(m));

            throw new InvalidMeasureUnitException(firstInvalidMeasureUnitId);
        }

        var brand = await _dbContext.Brands
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

        product.TechnicalDetails = request.TechnicalDetails
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

        _dbContext.Products.Add(product);

        await _dbContext.SaveChangesAsync(cancellationToken);

        //TODO:  Implement Retry with Polly and do publishing in other task for fast response
        await _publishEndpoint.Publish(ProductCreatedIntegrationEvent.CreateEvent(product.MapToProductDto()));

        return CreateProductCommandResponse.Respond($"products/{product.Id}", _configService);
    }
}
