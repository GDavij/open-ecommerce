using Core.Modules.Stock.Domain.IntegrationEvents.Models.Dtos;

namespace Core.Modules.Stock.Domain.IntegrationEvents.Brand;

internal class BrandUpdatedIntegrationEvent : BaseIntegrationEvent
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