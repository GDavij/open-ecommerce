namespace Core.Modules.Shared.Messaging.IntegrationEvents.Stock.Dtos;

public record ProductImageDto
{
    public Guid Id { get; init; }
    public Guid ProductId { get; init; }
    public string Description { get; init; }
    public string Url { get; init; }
    
    private ProductImageDto()
    {}

    public static ProductImageDto Create(
        Guid id,
        Guid productId,
        string description,
        string url)
    {
        return new ProductImageDto
        {
            Id = id,
            ProductId = productId,
            Description = description,
            Url = url
        };
    }
}