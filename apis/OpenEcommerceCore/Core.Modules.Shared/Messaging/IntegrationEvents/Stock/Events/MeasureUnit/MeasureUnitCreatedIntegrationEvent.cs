using Core.Modules.Shared.Messaging.IntegrationEvents.Stock.Dtos;

namespace Core.Modules.Shared.Messaging.IntegrationEvents.Stock.Events.MeasureUnit;

public class MeasureUnitCreatedIntegrationEvent 
{
    public MeasureUnitDto MeasureUnit { get; init; }
    
    private MeasureUnitCreatedIntegrationEvent()
    {}

    public static MeasureUnitCreatedIntegrationEvent CreateEvent(MeasureUnitDto measureUnitDto)
    {
        return new MeasureUnitCreatedIntegrationEvent
        {
            MeasureUnit = measureUnitDto
        };
    }
}