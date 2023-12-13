using Core.Modules.Stock.Domain.Entities.Complex.Product;

namespace Core.Modules.Stock.Domain.Entities;

internal class MeasureUnit
{
    public int Id { get; init; }
    public string Name { get; set; }
    public string? ShortName { get; set; }
    public string Symbol { get; set; }
    public List<ProductDetail> ProductDetails { get; set; }
    
    private MeasureUnit(
        int id,
        string name,
        string shortName,
        string symbol,
        List<ProductDetail> productDetails)
    {
        Id = id;
        Name = name;
        ShortName = shortName;
        Symbol = symbol;
        ProductDetails = productDetails;
    }

    public MeasureUnit Create(
        int id,
        string name,
        string shortName,
        string symbol,
        List<ProductDetail> productDetails = default)
    {
        return new MeasureUnit(
            id,
            name,
            shortName,
            symbol,
            productDetails);
    }
}
