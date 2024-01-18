using Core.Modules.Shared.Messaging.IntegrationEvents.Stock.Dtos;

namespace Core.Modules.Shared.Messaging.IntegrationEvents.Stock.Events.Brand;

public class BrandCreatedIntegrationEvent 
{
    public BrandDto Brand { get; init; }
    
    private BrandCreatedIntegrationEvent()
    {}

    public static BrandCreatedIntegrationEvent CreateEvent(BrandDto brandDto)
    {
        return new BrandCreatedIntegrationEvent
        {
            Brand = brandDto
        };
    }
}