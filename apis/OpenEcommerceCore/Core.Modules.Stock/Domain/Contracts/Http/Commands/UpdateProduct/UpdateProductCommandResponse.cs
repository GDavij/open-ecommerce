using Core.Modules.Shared.Domain.Contracts.Services;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.UpdateProduct;

public record UpdateProductCommandResponse
{
    public string Resource { get; init; }
    
    private UpdateProductCommandResponse()
    {}

    public static UpdateProductCommandResponse Respond(string resourcePath, IAppConfigService configService)
    {
        var url = configService.GetEnvironmentVariable("StockModule:AdministrativeDashboardBaseUrl");
        return new UpdateProductCommandResponse
        {
            Resource = $"{url}/{resourcePath}"
        };
    }
}