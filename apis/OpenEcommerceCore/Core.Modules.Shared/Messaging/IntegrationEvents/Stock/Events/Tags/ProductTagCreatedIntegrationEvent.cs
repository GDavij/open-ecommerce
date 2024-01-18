using Core.Modules.Shared.Messaging.IntegrationEvents.Stock.Dtos;

namespace Core.Modules.Shared.Messaging.IntegrationEvents.Stock.Events.Tags;

public class ProductTagCreatedIntegrationEvent 
{ 
    public  ProductTagDto Tag { get; init; }
    
    private ProductTagCreatedIntegrationEvent()
    {}

    public static ProductTagCreatedIntegrationEvent CreateEvent(ProductTagDto productTagDto)
    {
        return new ProductTagCreatedIntegrationEvent
        {
            Tag = productTagDto
        };
    }
}