using Core.Modules.Stock.Domain.IntegrationEvents.Models.Dtos;

namespace Core.Modules.Stock.Domain.IntegrationEvents.Tags;

public class ProductTagCreatedIntegrationEvent : BaseIntegrationEvent
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