namespace Core.Modules.Stock.Domain.Exceptions.Product;

internal class InvalidProductException : Exception
{
    public InvalidProductException(Guid productId) : base($"Invalid Product was sent with Id: {productId}")
    {}
}