namespace Core.Modules.Stock.Domain.Exceptions.Product;

internal class ExistentProductNameException : Exception
{
    public ExistentProductNameException(string invalidProductName) : base($"Found existent product name {invalidProductName}, Conflict Exception")
    {}
}