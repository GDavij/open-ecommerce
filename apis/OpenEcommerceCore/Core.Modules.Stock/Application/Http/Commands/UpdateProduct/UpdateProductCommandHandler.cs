using Core.Modules.Shared.Domain.Contracts.Services;
using Core.Modules.Stock.Domain.Contracts.Contexts;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.UpdateProduct;
using Core.Modules.Stock.Domain.Contracts.Providers;
using Core.Modules.Stock.Domain.Entities.Product;
using Core.Modules.Stock.Domain.Entities.Product.ProductDetails;
using Core.Modules.Stock.Domain.Exceptions.Product;
using Core.Modules.Stock.Domain.IntegrationEvents.Models.Mappings;
using Core.Modules.Stock.Domain.IntegrationEvents.Product;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.Stock.Application.Http.Commands.UpdateProduct;

internal class UpdateProductCommandHandler : IUpdateProductCommandHandler
{
    private readonly IStockContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IAppConfigService _configService;
    private readonly IStockDateTimeProvider _dateTimeProvider;

    public UpdateProductCommandHandler(
        IStockContext dbContext,
        IPublishEndpoint publishEndpoint,
        IAppConfigService configService,
        IStockDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
        _configService = configService;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var existentProduct = await _dbContext.Products
            .Include(c => c.Brand)
            .Include(c => c.Suppliers)
            .Include(c => c.Tags)
            .Include(c => c.Images)
            .Include(c => c.Measurements)
            .Include(c => c.TechnicalDetails)
            .Include(c => c.OtherDetails)
            .Include(c => c.ProductRestockDemands)
            .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

        if (existentProduct is null)
        {
            throw new InvalidProductException(request.ProductId);
        }

        var brandToUpdate = await _dbContext.Brands.FirstOrDefaultAsync(b => b.Id == request.BrandId, cancellationToken);
        if (brandToUpdate is null)
        {
            throw new InvalidBrandException(request.BrandId);
        }

        //Is Product Name Valid(Not Existent and different from existent one to update)
        if (await _dbContext.Products.AnyAsync(p =>
                    p.Name == request.Name &&
                    p.Id != request.ProductId,
                cancellationToken))
        {
            throw new ExistentProductNameException(request.Name);
        }

        if (await _dbContext.Products.AnyAsync(p =>
                    p.EAN == request.Ean &&
                    p.Id != request.ProductId,
                cancellationToken))
        {
            throw new ExistentEanCodeException(request.Ean);
        }

        if (request.Upc is not null &&
            await _dbContext.Products.AnyAsync(p =>
                    p.UPC == request.Upc &&
                    p.Id != request.ProductId,
                cancellationToken))
        {
            throw new ExistentUpcCodeException(request.Upc);
        }

        if (request.Sku is not null &&
            await _dbContext.Products.AnyAsync(p =>
                    p.SKU == request.Sku &&
                    p.Id != request.ProductId,
                cancellationToken))
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

        using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        existentProduct.Brand = brandToUpdate;
        existentProduct.Name = request.Name;
        existentProduct.Description = request.Description;
        existentProduct.SKU = request.Sku;
        existentProduct.EAN = request.Ean;
        existentProduct.UPC = request.Upc;
        existentProduct.Price = request.Price;
        existentProduct.StockUnitCount = request.StockUnitCount;
        existentProduct.Tags = validTags;
        existentProduct.LastUpdate = _dateTimeProvider.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        _dbContext.Products_MeasureDetails.RemoveRange(existentProduct.Measurements);

        var measureDetailsToAdd = request.Measurements.Select(m => MeasurementDetail.Create(
            product: existentProduct,
            showOrder: m.ShowOrder,
            name: m.Name,
            value: m.Value,
            measureUnit: validMeasureUnits.FirstOrDefault(vm => vm.Id == m.MeasureUnitId)));

        _dbContext.Products_MeasureDetails.AddRange(measureDetailsToAdd);

        _dbContext.Products_TechnicalDetails.RemoveRange(existentProduct.TechnicalDetails);

        var technicalDetailsToAdd = request.TechnicalDetails.Select(t => TechnicalDetail.Create(
            product: existentProduct,
            showOrder: t.ShowOrder,
            name: t.Name,
            value: t.Value,
            measureUnit: validMeasureUnits.FirstOrDefault(vm => vm.Id == t.MeasureUnitId)));

        _dbContext.Products_TechnicalDetails.AddRange(technicalDetailsToAdd);


        _dbContext.Products_OtherDetails.RemoveRange(existentProduct.OtherDetails);

        var otherDetailsToAdd = request.OtherDetails.Select(o => OtherDetail.Create(
            product: existentProduct,
            showOrder: o.ShowOrder,
            name: o.Name,
            value: o.Value,
            measureUnit: validMeasureUnits.FirstOrDefault(vm => vm.Id == o.MeasureUnitId)));

        _dbContext.Products_OtherDetails.AddRange(otherDetailsToAdd);


        await _dbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        //TODO: Add Retry With Polly
        await _publishEndpoint.Publish(ProductUpdatedIntegrationEvent.CreateEvent(existentProduct.MapToProductDto()));

        return UpdateProductCommandResponse.Respond($"products/{existentProduct.Id}", _configService);
    }
}