namespace Core.Modules.HumanResources.Domain.Contracts.DynamicData;

internal interface IDynamicDataAsyncResolver<T>
{
    Task<T> ResolveAsync();
}