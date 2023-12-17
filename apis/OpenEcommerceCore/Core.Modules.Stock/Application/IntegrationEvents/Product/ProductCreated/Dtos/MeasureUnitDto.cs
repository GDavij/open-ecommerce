namespace Core.Modules.Shared.Domain.IntegrationEvents.StockEvents.Product.ProductCreated.Dtos;

public class MeasureUnitDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string? ShortName { get; init; }
    public string Symbol { get; init; }
    
    private MeasureUnitDto()
    {}

    public static MeasureUnitDto Create(
        Guid id,
        string name,
        string shortName,
        string symbol)
    {
        return new MeasureUnitDto
        {

            Id = id,
            Name = name,
            ShortName = shortName,
            Symbol = symbol
        };
    }
}