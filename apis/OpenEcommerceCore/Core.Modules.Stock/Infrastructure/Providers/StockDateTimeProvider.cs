using Core.Modules.Stock.Domain.Contracts.Providers;

namespace Core.Modules.Stock.Infrastructure.Providers;

internal class StockDateTimeProvider : IStockDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}