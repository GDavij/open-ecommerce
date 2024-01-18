using Core.Modules.Shared.Messaging.IntegrationEvents.Stock.Dtos;

namespace Core.Modules.Shared.Messaging.IntegrationEvents.Stock.Events.MeasureUnit;

public class MeasureUnitUpdatedIntegrationEvent
{
    public MeasureUnitDto MeasureUnit { get; init; }
    
    private MeasureUnitUpdatedIntegrationEvent()
    {}

    public static MeasureUnitUpdatedIntegrationEvent CreateEvent(MeasureUnitDto measureUnitDto)
    {
        return new MeasureUnitUpdatedIntegrationEvent
        {
            MeasureUnit = measureUnitDto
        };
    }
}