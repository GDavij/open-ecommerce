namespace Core.Modules.Stock.Domain.Exceptions.Product;

internal class ExistentSkuCodeException : Exception
{
    public ExistentSkuCodeException(string skuAlias) : base($"Found a conflict with sku {skuAlias}, already exists!")
    {}
}