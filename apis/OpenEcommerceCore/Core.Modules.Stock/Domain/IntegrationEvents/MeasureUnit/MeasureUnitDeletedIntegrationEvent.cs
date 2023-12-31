namespace Core.Modules.Stock.Domain.IntegrationEvents.MeasureUnit;

public class MeasureUnitDeletedIntegrationEvent : BaseIntegrationEvent
{
    public Guid MeasureUnitId { get; init; }
    
    private MeasureUnitDeletedIntegrationEvent()
    {}

     public static MeasureUnitDeletedIntegrationEvent CreateEvent(Guid measureUnitId)
    {
        return new MeasureUnitDeletedIntegrationEvent
        {
            MeasureUnitId = measureUnitId
        };
    }
}