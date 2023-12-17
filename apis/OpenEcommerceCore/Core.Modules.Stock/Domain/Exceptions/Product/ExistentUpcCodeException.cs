namespace Core.Modules.Stock.Domain.Exceptions.Product;

internal class ExistentUpcCodeException : Exception
{
    public ExistentUpcCodeException(string upcCode) : base($"Found a conflict with upc {upcCode}, already exists!")
    {}
}