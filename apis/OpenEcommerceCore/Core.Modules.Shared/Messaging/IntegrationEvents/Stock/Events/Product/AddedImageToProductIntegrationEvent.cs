using Core.Modules.Shared.Messaging.IntegrationEvents.Stock.Dtos;

namespace Core.Modules.Shared.Messaging.IntegrationEvents.Stock.Events.Product;

public class AddedImageToProductIntegrationEvent 
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