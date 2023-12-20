namespace Core.Modules.Stock.Domain.Exceptions.Product;

internal class ProductBadRequestException : Exception
{
    public ProductBadRequestException(Guid invalidProductId) : base($"Could not find any product with {invalidProductId} Id, Bad Request")
    {}
}