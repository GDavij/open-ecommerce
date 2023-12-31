using Core.Modules.Stock.Domain.Entities.Product;
using Core.Modules.Stock.Domain.IntegrationEvents.Models.Dtos;

namespace Core.Modules.Stock.Domain.IntegrationEvents.Models.Mappings;

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