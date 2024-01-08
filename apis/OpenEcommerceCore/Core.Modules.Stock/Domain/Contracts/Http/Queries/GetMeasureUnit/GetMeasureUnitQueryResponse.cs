namespace Core.Modules.Stock.Domain.Contracts.Http.Queries.GetMeasureUnit;

public record GetMeasureUnitQueryResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string? ShortName { get; init; }
    public string Symbol { get; init; }
}