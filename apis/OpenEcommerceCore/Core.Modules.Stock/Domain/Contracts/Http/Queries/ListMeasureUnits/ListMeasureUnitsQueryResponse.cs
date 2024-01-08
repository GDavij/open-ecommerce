namespace Core.Modules.Stock.Domain.Contracts.Http.Queries.ListMeasureUnits;

public record ListMeasureUnitsQueryResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Symbol { get; init; }
}