using Core.Modules.Stock.Domain.IntegrationEvents.Models.Dtos;

namespace Core.Modules.Stock.Domain.IntegrationEvents.Product;

public class ProductCreatedIntegrationEvent : BaseIntegrationEvent
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