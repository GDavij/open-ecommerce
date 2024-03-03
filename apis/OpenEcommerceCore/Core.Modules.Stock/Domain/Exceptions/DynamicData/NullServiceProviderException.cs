namespace Core.Modules.Stock.Domain.Exceptions.DynamicData;

internal class NullServiceProviderException : Exception
{
    public NullServiceProviderException() : base("Found Http Context Resolution to Service Provider as Null")
    { }
}