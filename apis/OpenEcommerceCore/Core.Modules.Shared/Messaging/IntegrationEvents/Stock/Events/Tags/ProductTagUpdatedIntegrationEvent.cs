using Core.Modules.Shared.Messaging.IntegrationEvents.Stock.Dtos;

namespace Core.Modules.Shared.Messaging.IntegrationEvents.Stock.Events.Tags;

public class ProductTagUpdatedIntegrationEvent 
{
    public ProductTagDto Tag { get; init; }
    
    private ProductTagUpdatedIntegrationEvent()
    {}

    public static ProductTagUpdatedIntegrationEvent CreateEvent(ProductTagDto productTagDto)
    {
        return new ProductTagUpdatedIntegrationEvent
        {
            Tag = productTagDto
        };
    }
}