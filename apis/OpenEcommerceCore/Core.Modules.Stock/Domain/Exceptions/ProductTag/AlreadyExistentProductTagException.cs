namespace Core.Modules.Stock.Domain.Exceptions.ProductTag;

internal class AlreadyExistentProductTagException : Exception
{
    public AlreadyExistentProductTagException(string tagName) : base($"Found already existent Tag with Name {tagName}")
    { }
}