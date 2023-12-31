using Core.Modules.Stock.Domain.Contracts.Contexts;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.RemoveImageFromProduct;
using Core.Modules.Stock.Domain.Entities.Product;
using Core.Modules.Stock.Domain.Exceptions.Product;
using Core.Modules.Stock.Domain.IntegrationEvents.Product;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.Stock.Application.Http.Commands.RemoveImageFromProduct;

internal class RemoveImageFromProductCommandHandler : IRemoveImageFromProductCommandHandler
{
    private readonly IStockContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;

    public RemoveImageFromProductCommandHandler(
        IStockContext dbContext,
        IPublishEndpoint publishEndpoint)
    {
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<RemoveImageFromProductCommandResponse> Handle(RemoveImageFromProductCommand request, CancellationToken cancellationToken)
    {
        ProductImage? existentImage = await _dbContext.ProductImages
            .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        if (existentImage == null)
        {
            throw new ProductImageBadRequestException(request.Id);
        }

        var productId = existentImage.Product.Id;
        var imageId = existentImage.Id;

        _dbContext.ProductImages.Remove(existentImage);
        await _dbContext.SaveChangesAsync(cancellationToken);

        // Add Retry with Polly because it can happens data inconsistency
        await _publishEndpoint.Publish(RemoveImageFromProductIntegrationEvent.CreateEvent(productId, imageId));

        return RemoveImageFromProductCommandResponse.RespondWithSuccess();
    }
}