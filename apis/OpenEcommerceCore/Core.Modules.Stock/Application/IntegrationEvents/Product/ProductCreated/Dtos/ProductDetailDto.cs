namespace Core.Modules.Shared.Domain.IntegrationEvents.StockEvents.Product.ProductCreated.Dtos;

public record ProductDetailDto
{
    public Guid Id { get; init; }
    public Guid ProductId { get; init; }
    public int ShowOrder { get; init; }
    public string Name { get; init; }
    public string Value { get; init; }
    public MeasureUnitDto? MeasureUnitDto { get; init; }
    
    private ProductDetailDto()
    {}

    public static ProductDetailDto Create(
        Guid id,
        Guid productId,
        int showOrder,
        string name,
        string value,
        MeasureUnitDto? measureUnitDto)
    {
        return new ProductDetailDto
        {
            Id = id,
            ProductId = productId,
            ShowOrder = showOrder,
            Name = name,
            Value = value,
            MeasureUnitDto = measureUnitDto
        };
    }
}