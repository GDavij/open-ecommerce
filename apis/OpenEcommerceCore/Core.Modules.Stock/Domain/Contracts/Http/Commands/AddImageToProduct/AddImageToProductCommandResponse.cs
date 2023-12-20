using Core.Modules.Shared.Domain.Contracts.Services;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.AddImageToProduct;

public record AddImageToProductCommandResponse
{
    public string Resource { get; init; }
    
    private AddImageToProductCommandResponse()
    {}
    
    public static AddImageToProductCommandResponse Respond(string resourcePath, IAppConfigService configService)
    {
        var url = configService.GetEnvironmentVariable("StockModule:AdministrativeDashboardBaseUrl");
        return new AddImageToProductCommandResponse
        {
            Resource = $"{url}/{resourcePath}"
        };
    }
}