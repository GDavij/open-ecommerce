using Core.Modules.Stock.Domain.IntegrationEvents.Models.Dtos;

namespace Core.Modules.Stock.Domain.IntegrationEvents.Tags;

public class ProductTagUpdatedIntegrationEvent : BaseIntegrationEvent
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