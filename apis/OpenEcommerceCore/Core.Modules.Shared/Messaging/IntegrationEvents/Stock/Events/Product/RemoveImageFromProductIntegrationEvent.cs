namespace Core.Modules.Shared.Messaging.IntegrationEvents.Stock.Events.Product;

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