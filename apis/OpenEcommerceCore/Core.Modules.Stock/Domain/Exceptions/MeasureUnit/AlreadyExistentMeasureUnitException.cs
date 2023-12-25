namespace Core.Modules.Stock.Domain.Exceptions.MeasureUnit;

internal class AlreadyExistentMeasureUnitException : Exception
{
    public AlreadyExistentMeasureUnitException(string measureUnitName, string? measureUnitShortName) : base($"Found Existent Measure Unit with same Name {measureUnitName} or same ShortName {measureUnitShortName}, Conflict Exception")
    {}
}