namespace Core.Modules.Shared.Messaging.IntegrationEvents.Stock.Events.Product;

public class ProductDeletedIntegrationEvent 
{
    public Guid ProductId { get; init; }
    
    private ProductDeletedIntegrationEvent()
    {}

    public static ProductDeletedIntegrationEvent CreateEvent(Guid productId)
    {
        return new ProductDeletedIntegrationEvent
        {
            ProductId = productId
        };
    }
}