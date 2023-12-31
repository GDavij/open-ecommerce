using Core.Modules.Stock.Domain.IntegrationEvents.Models.Dtos;

namespace Core.Modules.Stock.Domain.IntegrationEvents.Product;

public class AddedImageToProductIntegrationEvent : BaseIntegrationEvent
{
    public ProductImageDto ProductImage { get; init; }
    
    private AddedImageToProductIntegrationEvent()
    {}

    public static AddedImageToProductIntegrationEvent CreateEvent(ProductImageDto productImage)
    {
        return new AddedImageToProductIntegrationEvent
        {
            ProductImage = productImage
        };
    }
}