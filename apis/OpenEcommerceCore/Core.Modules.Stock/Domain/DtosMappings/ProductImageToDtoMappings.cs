using Core.Modules.Shared.Messaging.IntegrationEvents.Stock.Dtos;
using Core.Modules.Stock.Domain.Entities.Product;

namespace Core.Modules.Stock.Domain.DtosMappings;

internal static class ProductImageToDtoMappings
{
    public static ProductImageDto MapToProductImageDto(this ProductImage image)
    {
        return ProductImageDto.Create(
            id: image.Id,
            productId: image.Product.Id,
            description: image.Description,
            url: image.Url);
    }
}