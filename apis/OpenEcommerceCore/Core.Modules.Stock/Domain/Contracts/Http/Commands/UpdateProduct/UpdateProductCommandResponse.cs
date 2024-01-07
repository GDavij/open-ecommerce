using Core.Modules.Shared.Domain.Contracts.Services;
using Core.Modules.Stock.Domain.Constants;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.UpdateProduct;

public record UpdateProductCommandResponse
{
    public string Resource { get; init; }
    
    private UpdateProductCommandResponse()
    {}

    public static UpdateProductCommandResponse Respond(string resourcePath, IAppConfigService configService)
    {
        var url = configService.GetEnvironmentVariable(StockModuleUrls.AdministrativeDashboardEnvironmentVariable);
        return new UpdateProductCommandResponse
        {
            Resource = $"{url}/{resourcePath}"
        };
    }
}