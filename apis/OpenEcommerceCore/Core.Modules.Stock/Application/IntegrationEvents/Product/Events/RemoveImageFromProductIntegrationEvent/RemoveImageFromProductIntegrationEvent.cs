using Core.Modules.Stock.Domain.Contracts.Http.Commands.RemoveImageFromProduct;

namespace Core.Modules.Stock.Application.IntegrationEvents.Product.Events.RemoveImageFromProductIntegrationEvent;

public class RemoveImageFromProductIntegrationEvent
{
    public Guid ProductId { get; init; }
    public Guid Id { get; init; }
    
    private RemoveImageFromProductIntegrationEvent()
    {}

    public static RemoveImageFromProductIntegrationEvent CreateEvent(Guid productId,Guid id)
    {
        return new RemoveImageFromProductIntegrationEvent
        {
            ProductId = productId,
            Id = id
        };
    }
}