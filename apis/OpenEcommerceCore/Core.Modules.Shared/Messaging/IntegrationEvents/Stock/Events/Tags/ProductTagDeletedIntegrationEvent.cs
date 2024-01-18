namespace Core.Modules.Shared.Messaging.IntegrationEvents.Stock.Events.Tags;

public class ProductTagDeletedIntegrationEvent 
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