namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.RemoveImageFromProduct;

internal record RemoveImageFromProductCommandResponse
{
    public bool Success { get; init; }
    
    private RemoveImageFromProductCommandResponse()
    {}

    public static RemoveImageFromProductCommandResponse RespondWithSuccess()
    {
        return new RemoveImageFromProductCommandResponse
        {
            Success = true
        };
    }
}