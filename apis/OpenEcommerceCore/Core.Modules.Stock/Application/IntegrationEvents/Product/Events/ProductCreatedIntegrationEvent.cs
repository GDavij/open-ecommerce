using Core.Modules.Stock.Application.IntegrationEvents;
using Core.Modules.Stock.Application.IntegrationEvents.Product.Dtos;

namespace Core.Modules.Shared.Domain.IntegrationEvents.StockEvents.Product.ProductCreated;

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