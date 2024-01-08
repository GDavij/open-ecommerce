namespace Core.Modules.Stock.Domain.IntegrationEvents.Tags;

internal class ProductTagDeletedIntegrationEvent : BaseIntegrationEvent
{
    public Guid TagId { get; init; }
    
    private ProductTagDeletedIntegrationEvent()
    {}

    public static ProductTagDeletedIntegrationEvent CreateEvent(Guid tagId)
    {
        return new ProductTagDeletedIntegrationEvent
        {
            TagId = tagId
        };
    }
}