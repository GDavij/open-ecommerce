using Core.Modules.Stock.Application.IntegrationEvents.Product.Dtos;

namespace Core.Modules.Stock.Application.IntegrationEvents.MeasureUnit;

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