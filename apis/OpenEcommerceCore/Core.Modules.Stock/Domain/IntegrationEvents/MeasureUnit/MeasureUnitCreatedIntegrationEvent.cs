using Core.Modules.Stock.Domain.IntegrationEvents.Models.Dtos;

namespace Core.Modules.Stock.Domain.IntegrationEvents.MeasureUnit;

internal class MeasureUnitCreatedIntegrationEvent : BaseIntegrationEvent
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