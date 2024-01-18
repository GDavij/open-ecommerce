using Core.Modules.Shared.Messaging.IntegrationEvents.Stock.Dtos;
using Core.Modules.Stock.Domain.Entities;

namespace Core.Modules.Stock.Domain.DtosMappings;

internal static class MeasureUnitToDtoMappings
{
    public static MeasureUnitDto MapToMeasureUnitDto(this MeasureUnit measureUnit)
    {
        return MeasureUnitDto.Create(
            id: measureUnit.Id, 
            name: measureUnit.Name,
            shortName: measureUnit.ShortName,
            symbol: measureUnit.Symbol);
    }
}