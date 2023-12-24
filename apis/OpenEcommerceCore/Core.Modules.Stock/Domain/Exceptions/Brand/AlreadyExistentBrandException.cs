namespace Core.Modules.Stock.Domain.Exceptions.Brand;

internal class AlreadyExistentBrandException : Exception
{
    public AlreadyExistentBrandException(string brandName) : base($"Found an existent Brand with {brandName} Name, conflict exception")
    {}
}