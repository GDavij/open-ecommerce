namespace Core.Modules.Stock.Domain.Entities;

internal class MeasureUnit
{
    public Guid Id { get; init; }
    public string Name { get; set; }
    public string? ShortName { get; set; }
    public string Symbol { get; set; }
    
    private MeasureUnit()
    {}

    public static MeasureUnit Create(
        string name,
        string shortName,
        string symbol)
    {
        return new MeasureUnit
        {

            Id = Guid.NewGuid(),
            Name = name,
            ShortName = shortName,
            Symbol = symbol
        };
    }
}
