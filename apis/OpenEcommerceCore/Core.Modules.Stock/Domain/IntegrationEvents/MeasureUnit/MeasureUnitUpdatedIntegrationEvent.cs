using Core.Modules.Stock.Domain.IntegrationEvents.Models.Dtos;

namespace Core.Modules.Stock.Domain.IntegrationEvents.MeasureUnit;

public class MeasureUnitUpdatedIntegrationEvent : BaseIntegrationEvent
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