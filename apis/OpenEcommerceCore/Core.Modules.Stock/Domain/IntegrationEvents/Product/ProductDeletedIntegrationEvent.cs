namespace Core.Modules.Stock.Domain.IntegrationEvents.Product;

internal class ProductDeletedIntegrationEvent : BaseIntegrationEvent
{
    public Guid ProductId { get; init; }
    
    private ProductDeletedIntegrationEvent()
    {}

    public static ProductDeletedIntegrationEvent CreateEvent(Guid productId)
    {
        return new ProductDeletedIntegrationEvent
        {
            ProductId = productId
        };
    }
}