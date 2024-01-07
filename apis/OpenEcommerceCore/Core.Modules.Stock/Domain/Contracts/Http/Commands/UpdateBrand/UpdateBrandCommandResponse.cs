using Core.Modules.Shared.Domain.Contracts.Services;
using Core.Modules.Stock.Domain.Constants;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.UpdateBrand;

public record UpdateBrandCommandResponse
{
    public string Resource { get; init; }
    
    private UpdateBrandCommandResponse()
    {}
    
    public static UpdateBrandCommandResponse Respond(string resourcePath, IAppConfigService configService)
    {
        var url = configService.GetEnvironmentVariable(StockModuleUrls.AdministrativeDashboardEnvironmentVariable);
        return new UpdateBrandCommandResponse
        {
            Resource = $"{url}/{resourcePath}"
        };
    }
};