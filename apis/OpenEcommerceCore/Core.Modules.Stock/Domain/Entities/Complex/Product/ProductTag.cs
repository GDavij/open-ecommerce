namespace Core.Modules.Stock.Domain.Entities.Complex.Product;

internal class ProductTag
{
    public Guid Id { get; init; }
    public string Name { get; set; }
    public List<Product> TaggedProducts { get; set; }

    private ProductTag(Guid id, string name, List<Product> taggedProducts)
    {
        Id = id;
        Name = name;
        TaggedProducts = taggedProducts;
    }

    public ProductTag Create(Guid id, string name, List<Product> taggedProducts)
    {
        return new ProductTag(id, name, taggedProducts);
    }
}