namespace Core.Modules.Shared.Messaging.IntegrationEvents.Stock.Events.MeasureUnit;

public class MeasureUnitDeletedIntegrationEvent 
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