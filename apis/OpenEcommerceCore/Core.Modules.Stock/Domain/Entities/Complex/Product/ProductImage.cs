namespace Core.Modules.Stock.Domain.Entities.Complex.Product;

internal class ProductImage
{
    public Guid Id { get; init; }
    public Product Product { get; init; }
    public string Description { get; init; }
    public string Url { get; init; }

    private ProductImage(Guid id, Product product,string description, string url)
    {
        Id = id;
        Product = product;
        Description = description;
        Url = url;
    }

    //TODO: Maybe make a Overload to accepts Blob and Description and creates a Image (Move Logic to Class Basically)
    public ProductImage Create(Product product,  string description, string url)
    {
        return new ProductImage(Guid.NewGuid(), product, description, url);
    }
}