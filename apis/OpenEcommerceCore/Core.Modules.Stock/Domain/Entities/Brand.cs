using Core.Modules.Stock.Domain.Entities.Complex.Product;

namespace Core.Modules.Stock.Domain.Entities;

internal class Brand
{
    public Guid Id { get; init; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    // Relationships
    public List<Product> Products { get; set; } = new List<Product>();

    private Brand()
    {}

    public static Brand Create(
        string name,
        string description)
    {
        return new Brand
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description
        };
    }
}