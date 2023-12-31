using Core.Modules.Stock.Domain.IntegrationEvents.Models.Dtos;

namespace Core.Modules.Stock.Domain.IntegrationEvents.Product;

public class ProductUpdatedIntegrationEvent : BaseIntegrationEvent
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