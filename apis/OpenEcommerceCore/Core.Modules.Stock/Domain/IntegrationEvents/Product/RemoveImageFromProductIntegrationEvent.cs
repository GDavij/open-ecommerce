namespace Core.Modules.Stock.Domain.IntegrationEvents.Product;

public class RemoveImageFromProductIntegrationEvent : BaseIntegrationEvent
{
    public Guid ProductId { get; init; }
    public Guid Id { get; init; }
    
    private RemoveImageFromProductIntegrationEvent()
    {}

    public static RemoveImageFromProductIntegrationEvent CreateEvent(Guid productId,Guid id)
    {
        return new RemoveImageFromProductIntegrationEvent
        {
            ProductId = productId,
            Id = id
        };
    }
}