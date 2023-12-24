namespace Core.Modules.Stock.Domain.Exceptions.Product;

internal class InvalidBrandException : Exception
{
    public InvalidBrandException(Guid brandId) : base($"Invalid Brand was sent with id: {brandId}")
    {}
}