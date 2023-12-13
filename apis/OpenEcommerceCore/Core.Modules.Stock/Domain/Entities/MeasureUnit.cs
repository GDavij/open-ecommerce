using Core.Modules.Stock.Domain.Entities.Complex.Product;

namespace Core.Modules.Stock.Domain.Entities;

internal class MeasureUnit
{
    public int Id { get; init; }
    public string Name { get; set; }
    public string? ShortName { get; set; }
    public string Symbol { get; set; }
    
    private MeasureUnit(
        int id,
        string name,
        string shortName,
        string symbol)
    {
        Id = id;
        Name = name;
        ShortName = shortName;
        Symbol = symbol;
    }

    public MeasureUnit Create(
        int id,
        string name,
        string shortName,
        string symbol)
    {
        return new MeasureUnit(
            id,
            name,
            shortName,
            symbol);
    }
}
