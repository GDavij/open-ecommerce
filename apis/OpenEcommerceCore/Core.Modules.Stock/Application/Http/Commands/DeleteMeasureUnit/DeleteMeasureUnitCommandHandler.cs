using Core.Modules.Shared.Messaging.IntegrationEvents.Stock.Events.MeasureUnit;
using Core.Modules.Stock.Domain.Contracts.Contexts;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.DeleteMeasureUnit;
using Core.Modules.Stock.Domain.Exceptions.Product;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.Stock.Application.Http.Commands.DeleteMeasureUnit;

internal class DeleteMeasureUnitCommandHandler : IDeleteMeasureUnitUseCase
{
    private readonly IStockContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;

    public DeleteMeasureUnitCommandHandler(IStockContext dbContext, IPublishEndpoint publishEndpoint)
    {
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(DeleteMeasureUnitCommand request, CancellationToken cancellationToken)
    {
        var existentMeasureUnit = await _dbContext.MeasureUnits
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        if (existentMeasureUnit is null)
        {
            throw new InvalidMeasureUnitException(request.Id);
        }

        _dbContext.MeasureUnits.Remove(existentMeasureUnit);

        await _dbContext.SaveChangesAsync(cancellationToken);
        
        //TODO: Add Retry
        await _publishEndpoint.Publish(MeasureUnitDeletedIntegrationEvent.CreateEvent(request.Id));
    }
}