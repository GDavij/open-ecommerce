using Core.Modules.Shared.Domain.Contracts.Services;
using Core.Modules.Stock.Domain.Constants;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.UpdateProductTag;

public record UpdateProductTagCommandResponse
{
    public string Resource { get; init; }
        
    private UpdateProductTagCommandResponse()
    {}
    
    public static UpdateProductTagCommandResponse Respond(string resourcePath, IAppConfigService configService)
    {
        var url = configService.GetEnvironmentVariable(StockModuleUrls.AdministrativeDashboardEnvironmentVariable);
        return new UpdateProductTagCommandResponse
        {
            Resource = $"{url}/{resourcePath}"
        };
    }
}