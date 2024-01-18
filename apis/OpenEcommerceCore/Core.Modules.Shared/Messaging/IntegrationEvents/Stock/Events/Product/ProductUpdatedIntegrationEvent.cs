using Core.Modules.Shared.Messaging.IntegrationEvents.Stock.Dtos;

namespace Core.Modules.Shared.Messaging.IntegrationEvents.Stock.Events.Product;

public class ProductUpdatedIntegrationEvent 
{
    public ProductDto Product { get; init; }
    
    private ProductUpdatedIntegrationEvent()
    {}

    public static ProductUpdatedIntegrationEvent CreateEvent(ProductDto productDto)
    {
        return new ProductUpdatedIntegrationEvent
        {
            Product = productDto
        };
    }
}