using Core.Modules.Shared.Domain.Contracts.Services;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.CreateProduct;

public record CreateProductCommandResponse
{
    public string Resource { get; init; }
    
    private CreateProductCommandResponse()
    {}

    public static CreateProductCommandResponse Respond(string resourcePath, IAppConfigService configService)
    {
        var url = configService.GetEnvironmentVariable("StockModule:AdministrativeDashboardBaseUrl");
        return new CreateProductCommandResponse
        {
            Resource = $"{url}/{resourcePath}"
        };
    }
}