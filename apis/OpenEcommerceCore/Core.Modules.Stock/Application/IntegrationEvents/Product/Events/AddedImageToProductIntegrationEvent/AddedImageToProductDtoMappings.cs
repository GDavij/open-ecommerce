using Core.Modules.Stock.Application.IntegrationEvents.Product.Dtos;
using Core.Modules.Stock.Domain.Entities.Product;

namespace Core.Modules.Stock.Application.IntegrationEvents.Product.Events.AddedImageToProductIntegrationEvent;

internal static class  AddedImageToProductDtoMappings
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