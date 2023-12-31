namespace Core.Modules.Stock.Domain.IntegrationEvents.Brand;

internal class BrandDeletedIntegrationEvent : BaseIntegrationEvent
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