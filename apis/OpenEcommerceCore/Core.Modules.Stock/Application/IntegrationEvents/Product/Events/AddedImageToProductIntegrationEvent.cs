using Core.Modules.Stock.Application.IntegrationEvents.Product.Dtos;

namespace Core.Modules.Stock.Application.IntegrationEvents.Product.Events.AddedImageToProductIntegrationEvent;

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