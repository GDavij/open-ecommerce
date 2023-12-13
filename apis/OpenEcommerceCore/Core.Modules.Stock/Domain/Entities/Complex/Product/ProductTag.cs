namespace Core.Modules.Stock.Domain.Entities.Complex.Product;

internal class ProductTag
{
    public Guid Id { get; init; }
    public string Name { get; set; }

    private ProductTag(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public ProductTag Create(Guid id, string name)
    {
        return new ProductTag(id, name);
    }
}