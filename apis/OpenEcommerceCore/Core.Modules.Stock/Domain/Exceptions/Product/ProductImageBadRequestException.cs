namespace Core.Modules.Stock.Domain.Exceptions.Product;

internal class ProductImageBadRequestException : Exception
{
    public ProductImageBadRequestException(Guid productImageId) : base($"Could not find any product image with {productImageId} Id, Bad Request")
    {}
}