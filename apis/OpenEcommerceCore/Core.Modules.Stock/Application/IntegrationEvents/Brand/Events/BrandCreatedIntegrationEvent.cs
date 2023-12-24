using Core.Modules.Stock.Application.IntegrationEvents.Product.Dtos;

namespace Core.Modules.Stock.Application.IntegrationEvents.Brand.Events;

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