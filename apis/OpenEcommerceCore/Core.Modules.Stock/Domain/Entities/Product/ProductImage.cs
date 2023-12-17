namespace Core.Modules.Stock.Domain.Entities.Product;

internal class ProductImage
{
    public Guid Id { get; init; }
    public Entities.Product.Product Product { get; init; }
    public string Description { get; init; }
    public string Url { get; init; }

    private ProductImage()
    {}

    //TODO: Maybe make a Overload to accepts Blob and Description and creates a Image in the file storage Method, Use Strategy Pattern to File Storage(Move Logic to Class Basically)
    public static ProductImage Create(Guid id, Product product, string description, string url)
    {
        return new ProductImage
        {
           Id = id,
           Product = product,
           Description = description,
           Url = url
        };
    }
}