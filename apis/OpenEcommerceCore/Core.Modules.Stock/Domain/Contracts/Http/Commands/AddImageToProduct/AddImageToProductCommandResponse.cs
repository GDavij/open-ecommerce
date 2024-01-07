using Core.Modules.Shared.Domain.Contracts.Services;
using Core.Modules.Stock.Domain.Constants;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.AddImageToProduct;

public record AddImageToProductCommandResponse
{
    public string Resource { get; init; }
    
    private AddImageToProductCommandResponse()
    {}
    
    public static AddImageToProductCommandResponse Respond(string resourcePath, IAppConfigService configService)
    {
        var url = configService.GetEnvironmentVariable(StockModuleUrls.AdministrativeDashboardEnvironmentVariable);
        return new AddImageToProductCommandResponse
        {
            Resource = $"{url}/{resourcePath}"
        };
    }
}