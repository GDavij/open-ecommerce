using Core.Modules.Stock.Domain.Entities.Product;
using Core.Modules.Stock.Domain.IntegrationEvents.Models.Dtos;

namespace Core.Modules.Stock.Domain.IntegrationEvents.Models.Mappings;

internal static class ProductTagToDtoMappings
{
    public static ProductTagDto MapToProductTagDto(this ProductTag productTag)
    {
        return ProductTagDto.Create(
            productTag.Id,
            productTag.Name,
            productTag.TaggedProducts.Select(p => p.Id).ToList());
    }
}