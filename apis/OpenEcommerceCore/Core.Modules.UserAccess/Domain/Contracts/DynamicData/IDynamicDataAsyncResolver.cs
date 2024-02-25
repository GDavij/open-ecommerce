namespace Core.Modules.UserAccess.Domain.Contracts.DynamicData;

internal interface IDynamicDataAsyncResolver<T>
{
    Task<T> ResolveAsync();
}