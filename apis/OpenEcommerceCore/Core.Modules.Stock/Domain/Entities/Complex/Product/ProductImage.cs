namespace Core.Modules.Stock.Domain.Entities.Complex.Product;

internal class ProductImage
{
    public Guid Id { get; init; }
    public string Description { get; init; }
    public string Url { get; init; }

    private ProductImage(Guid id,  string description, string url)
    {
        Id = id;
        Description = description;
        Url = url;
    }

    //TODO: Maybe make a Overload to accepts Blob and Description and creates a Image (Move Logic to Class Basically)
    public ProductImage Create(Guid id,  string description, string url)
    {
        return new ProductImage(id, description, url);
    }
}