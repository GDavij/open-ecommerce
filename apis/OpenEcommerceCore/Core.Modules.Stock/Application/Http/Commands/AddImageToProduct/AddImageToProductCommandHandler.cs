using Azure.Storage.Blobs;
using Core.Modules.Shared.Domain.Constants;
using Core.Modules.Shared.Domain.Contracts.Services;
using Core.Modules.Stock.Domain.Contracts.Contexts;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.AddImageToProduct;
using Core.Modules.Stock.Domain.Entities.Product;
using Core.Modules.Stock.Domain.Exceptions.Product;
using Core.Modules.Stock.Domain.IntegrationEvents.Models.Mappings;
using Core.Modules.Stock.Domain.IntegrationEvents.Product;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.Stock.Application.Http.Commands.AddImageToProduct;

internal class AddImageToProductCommandHandler : IAddImageToProductCommandHandler
{
    private readonly IStockContext _dbContext;
    private readonly BlobServiceClient _blobServiceClient;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IAppConfigService _appConfigService;

    public AddImageToProductCommandHandler(
        IStockContext dbContext,
        BlobServiceClient blobServiceClient,
        IPublishEndpoint publishEndpoint,
        IAppConfigService appConfigService)
    {
        _dbContext = dbContext;
        _blobServiceClient = blobServiceClient;
        _publishEndpoint = publishEndpoint;
        _appConfigService = appConfigService;
    }
    
    public async Task<AddImageToProductCommandResponse> Handle(AddImageToProductCommand request, CancellationToken cancellationToken)
    {
        var existentProduct = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);
        if (existentProduct is null)
        {
            throw new ProductBadRequestException(request.ProductId);
        }

        var container = _blobServiceClient.GetBlobContainerClient(AzureBlobStorageContainers.ProductImages);
        
        string fileType = request.ImageFile.ContentType.Split('/')[1];

        string blobName = $"{Guid.NewGuid()}.{fileType}";
        
        await container.UploadBlobAsync(blobName, request.ImageFile.OpenReadStream(), cancellationToken);
        
        var productImage = ProductImage.Create(
            product: existentProduct,
            description: request.Description,
            url: $"{container.Uri.AbsoluteUri}/{blobName}");

        _dbContext.ProductImages.Add(productImage);

        // Add Retry for Db Context
        await _dbContext.SaveChangesAsync();

        // Add Retry With Polly - Eventual Consistency Warranty
        await _publishEndpoint.Publish(AddedImageToProductIntegrationEvent.CreateEvent(productImage.MapToProductImageDto()));

        return AddImageToProductCommandResponse.Respond($"products/{existentProduct.Id}", _appConfigService);
    }
}