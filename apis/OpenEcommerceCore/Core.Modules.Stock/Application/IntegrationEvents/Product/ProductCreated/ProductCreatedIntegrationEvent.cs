using Core.Modules.Shared.Domain.IntegrationEvents.StockEvents.Product.ProductCreated.Dtos;
using Core.Modules.Stock.Application.IntegrationEvents;

namespace Core.Modules.Shared.Domain.IntegrationEvents.StockEvents.Product.ProductCreated;

internal class ProductCreatedIntegrationEvent : BaseIntegrationEvent
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