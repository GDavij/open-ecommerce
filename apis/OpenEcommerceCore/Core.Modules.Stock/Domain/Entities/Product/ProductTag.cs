namespace Core.Modules.Stock.Domain.Entities.Complex.Product;

internal class ProductTag
{
    public Guid Id { get; init; }
    public string Name { get; set; }
    
    // Relationships
    public List<Product> TaggedProducts { get; set; } = new List<Product>();

    private ProductTag()
    {}

    public static ProductTag Create(string name)
    {
        return new ProductTag
        {
            Id = Guid.NewGuid(),
            Name = name
        };
    }
}