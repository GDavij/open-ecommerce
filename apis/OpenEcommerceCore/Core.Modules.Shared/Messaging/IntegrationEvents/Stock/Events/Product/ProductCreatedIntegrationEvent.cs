using Core.Modules.Shared.Messaging.IntegrationEvents.Stock.Dtos;

namespace Core.Modules.Shared.Messaging.IntegrationEvents.Stock.Events.Product;

public class ProductCreatedIntegrationEvent 
{
    public ProductDto Product { get; private set; }
    
    private ProductCreatedIntegrationEvent()
    {}

    public static ProductCreatedIntegrationEvent CreateEvent(ProductDto product)
    {
        return new ProductCreatedIntegrationEvent
        {
            Product = product
        };
    }
}