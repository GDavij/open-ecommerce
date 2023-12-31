using Core.Modules.Stock.Domain.IntegrationEvents.Models.Dtos;

namespace Core.Modules.Stock.Domain.IntegrationEvents.Models.Mappings;

internal static class MeasureUnitToDtoMappings
{
    public static MeasureUnitDto MapToMeasureUnitDto(this Domain.Entities.MeasureUnit measureUnit)
    {
        return MeasureUnitDto.Create(
            id: measureUnit.Id, 
            name: measureUnit.Name,
            shortName: measureUnit.ShortName,
            symbol: measureUnit.Symbol);
    }
}