namespace Core.Modules.Stock.Domain.Contracts.DynamicData;

internal interface IDynamicDataAsyncResolver<T>
{
    Task<T> ResolveAsync();
}