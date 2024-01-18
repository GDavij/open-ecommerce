using Core.Modules.Shared.Messaging.IntegrationEvents.Stock.Events.Product;
using Core.Modules.Stock.Domain.Contracts.Contexts;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.DeleteProduct;
using Core.Modules.Stock.Domain.Exceptions.Product;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.Stock.Application.Http.Commands.DeleteProduct;

internal class DeleteProductCommandHandler : IDeleteProductCommandHandler
{
    private readonly IStockContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;

    public DeleteProductCommandHandler(IStockContext dbContext, IPublishEndpoint publishEndpoint)
    {
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var existentProduct = await _dbContext.Products
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (existentProduct is null)
        {
            throw new InvalidProductException(request.Id);
        }

        _dbContext.Products.Remove(existentProduct);

        await _dbContext.SaveChangesAsync(cancellationToken);

        // Add Retry With Polly
        _publishEndpoint.Publish(ProductDeletedIntegrationEvent.CreateEvent(request.Id));
    }
}