using Core.Modules.Stock.Application.IntegrationEvents.Product.Dtos;

namespace Core.Modules.Stock.Application.IntegrationEvents.Models.Mappings;

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