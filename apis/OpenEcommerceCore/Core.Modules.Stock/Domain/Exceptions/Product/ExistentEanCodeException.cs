namespace Core.Modules.Stock.Domain.Exceptions.Product;

internal class ExistentEanCodeException : Exception
{
    public ExistentEanCodeException(string eanCode) : base($"Found a conflict with ean-13 {eanCode}, already exists!")
    {}
}