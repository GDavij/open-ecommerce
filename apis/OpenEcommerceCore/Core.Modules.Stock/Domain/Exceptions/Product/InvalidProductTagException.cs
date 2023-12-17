namespace Core.Modules.Stock.Domain.Exceptions.Product;

internal class InvalidProductTagException : Exception
{
    public InvalidProductTagException(Guid tagId) : base($"Invalid Tag was sent, id is {tagId}")
    {}
}