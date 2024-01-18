using Core.Modules.Shared.Messaging.IntegrationEvents.Stock.Dtos;
using Core.Modules.Stock.Domain.Entities.Product;

namespace Core.Modules.Stock.Domain.DtosMappings;

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