using Core.Modules.Shared.Messaging.IntegrationEvents.Stock.Events.Brand;
using Core.Modules.Stock.Domain.Contracts.Contexts;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.DeleteBrand;
using Core.Modules.Stock.Domain.Exceptions.Product;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.Stock.Application.Http.Commands.DeleteBrand;

internal class DeleteBrandCommandHandler : IDeleteBrandCommandHandler
{
    private readonly IStockContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;

    public DeleteBrandCommandHandler(IStockContext dbContext, IPublishEndpoint publishEndpoint)
    {
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
    {
        var existentBrand = await _dbContext.Brands
            .FirstOrDefaultAsync(b => b.Id == request.Id);

        if (existentBrand is null)
        {
            throw new InvalidBrandException(request.Id);
        }

        _dbContext.Brands.Remove(existentBrand);

        await _dbContext.SaveChangesAsync(cancellationToken);

        //TODO: Add Retry With Polly
        await _publishEndpoint.Publish(BrandDeletedIntegrationEvent.CreateEvent(request.Id));
    }
}