namespace Core.Modules.Shared.Domain.IntegrationEvents.StockEvents.Product.ProductCreated.Dtos;

internal class BrandDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public List<Guid> ProductsIds { get; init; } = new List<Guid>();
    
    private BrandDto()
    {}

    public static BrandDto Create(
        Guid id,
        string name,
        string description,
        List<Guid> productsIds)
    {
        return new BrandDto
        {
            Id = id,
            Name = name,
            Description = description,
            ProductsIds = productsIds
        };
    }
}