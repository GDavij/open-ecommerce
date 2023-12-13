namespace Core.Modules.Stock.Domain.Entities.Complex.Product;

internal class ProductDetail
{
    public Guid Id { get; init; }
    public Product Product { get; init; }
    public int ShowOrder { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
    public MeasureUnit? MeasureUnit { get; set; }

    private ProductDetail(
        Guid id,
        Product product,
        int showOrder,
        string name,
        string value,
        MeasureUnit? measureUnit)
    {
        Id = id;
        Product = product;
        ShowOrder = showOrder;
        Name = name;
        Value = value;
        MeasureUnit = measureUnit;
    }

    public ProductDetail Create(
        Guid id,
        Product product,
        int showOrder,
        string name,
        string value,
        MeasureUnit? measureUnit = null)
    {
        return new ProductDetail(
            id,
            product,
            showOrder,
            name,
            value,
            measureUnit);
    }
}