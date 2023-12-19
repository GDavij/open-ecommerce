namespace Core.Modules.Stock.Domain.Exceptions.Product;

internal class ShowOrderRepeatedException : Exception
{
    public ShowOrderRepeatedException(string listEncountered) : base($"Encountered repeated value at list {listEncountered}")
    {}
}

internal static class ShowOrderRepeatedEncountered
{
    public const string Measures = "Measures";
    public const string TechnicalDetails = "Technical Details";
    public const string OtherDetails = "Other Details";
}