namespace Core.Modules.Shared.Messaging.IntegrationEvents.Stock.Dtos;

public record ProductTagDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public List<Guid> TaggedProductsIds { get; init; } = new List<Guid>();
    
    private ProductTagDto()
    {}

    public static ProductTagDto Create(
        Guid id,
        string name,
        List<Guid> taggedProductsIds)
    {
        return new ProductTagDto
        {
            Id = id,
            Name = name,
            TaggedProductsIds = taggedProductsIds
        };
    }

}