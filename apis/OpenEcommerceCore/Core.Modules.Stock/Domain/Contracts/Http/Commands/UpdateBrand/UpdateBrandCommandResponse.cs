using Core.Modules.Shared.Domain.Contracts.Services;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.UpdateBrand;

public record UpdateBrandCommandResponse
{
    public string Resource { get; init; }
    
    private UpdateBrandCommandResponse()
    {}
    
    public static UpdateBrandCommandResponse Respond(string resourcePath, IAppConfigService configService)
    {
        var url = configService.GetEnvironmentVariable("StockModule:AdministrativeDashboardBaseUrl");
        return new UpdateBrandCommandResponse
        {
            Resource = $"{url}/{resourcePath}"
        };
    }
};