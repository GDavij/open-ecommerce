namespace Core.Modules.Shared.Messaging.IntegrationEvents.Stock.Events.Brand;

public class BrandDeletedIntegrationEvent
{
    public Guid BrandId { get; init; }
    
    private BrandDeletedIntegrationEvent()
    {}

    public static BrandDeletedIntegrationEvent CreateEvent(Guid brandId)
    {
        return new BrandDeletedIntegrationEvent
        {
            BrandId = brandId
        };
    }
}