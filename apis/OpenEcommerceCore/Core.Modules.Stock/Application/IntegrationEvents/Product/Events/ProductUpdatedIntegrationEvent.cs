using Core.Modules.Stock.Application.IntegrationEvents.Product.Dtos;

namespace Core.Modules.Stock.Application.IntegrationEvents.Product.Events;

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