namespace Core.Modules.Stock.Domain.Exceptions.Product;

internal class InvalidMeasureUnitException : Exception
{
    public InvalidMeasureUnitException(Guid measureUnitId) : base($"Invalid Measure Unit was sent with Id: {measureUnitId}")
    {}
}