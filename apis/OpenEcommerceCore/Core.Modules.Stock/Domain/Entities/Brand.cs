using Core.Modules.Stock.Domain.Entities.Complex.Product;

namespace Core.Modules.Stock.Domain.Entities;

internal class Brand
{
    public Guid Id { get; init; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<Product> Products { get; set; }

    private Brand(
        Guid id,
        string name,
        string description,
        List<Product> products)
    {
        Id = id;
        Name = name;
        Description = description;
        Products = products;
    }

    public Brand Create(
        Guid id,
        string name,
        string description)
    {
        return new Brand(
            id,
            name,
            description,
            new List<Product>());
    }
}