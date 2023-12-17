namespace Core.Modules.Stock.Domain.Contracts.Providers;

internal interface IStockDateTimeProvider
{
    public DateTime UtcNow { get; }
}