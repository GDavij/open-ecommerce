using Core.Modules.Stock.Domain.Contracts.Contexts;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.DeleteProductTag;
using Core.Modules.Stock.Domain.Exceptions.Product;
using Core.Modules.Stock.Domain.IntegrationEvents.Tags;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.Stock.Application.Http.Commands.DeleteProductTag;

internal class DeleteProductTagCommandHandler : IDeleteProductTagCommandHandler
{
    private readonly IStockContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;
    
    public DeleteProductTagCommandHandler(IStockContext dbContext, IPublishEndpoint publishEndpoint)
    {
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(DeleteProductTagCommand request, CancellationToken cancellationToken)
    {
        var existentProductTag = await _dbContext.ProductTags
            .FirstOrDefaultAsync(pt => pt.Id == request.Id, cancellationToken);

        if (existentProductTag is null)
        {
            throw new InvalidProductTagException(request.Id);
        }

        _dbContext.ProductTags.Remove(existentProductTag);

        await _dbContext.SaveChangesAsync(cancellationToken);

        //TODO: Add Retry with Polly
        await _publishEndpoint.Publish(ProductTagDeletedIntegrationEvent.CreateEvent(request.Id));
    }
}