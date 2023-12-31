using Core.Modules.Stock.Domain.IntegrationEvents.Models.Dtos;

namespace Core.Modules.Stock.Domain.IntegrationEvents.Brand;

internal class BrandCreatedIntegrationEvent : BaseIntegrationEvent
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