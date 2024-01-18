using Core.Modules.Shared.Messaging.IntegrationEvents.Stock.Dtos;

namespace Core.Modules.Shared.Messaging.IntegrationEvents.Stock.Events.Brand;

public class BrandUpdatedIntegrationEvent
{
    public BrandDto Brand { get; init; }
    
    private BrandUpdatedIntegrationEvent()
    {}

    public static BrandUpdatedIntegrationEvent CreateEvent(BrandDto brand)
    {
        return new BrandUpdatedIntegrationEvent
        {
            Brand = brand
        };
    }
}