using Core.Modules.Shared.Domain.Contracts.Services;
using Core.Modules.Stock.Domain.Constants;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.UpdateMeasureUnit;

public record UpdateMeasureUnitCommandResponse
{
    public string Resource { get; init; }
    
    private UpdateMeasureUnitCommandResponse()
    {}

    public static UpdateMeasureUnitCommandResponse Respond(string resourcePath, IAppConfigService configService)
    {
        var url = configService.GetEnvironmentVariable(StockModuleUrls.AdministrativeDashboardEnvironmentVariable);
        return new UpdateMeasureUnitCommandResponse
        {
            Resource = $"{url}/{resourcePath}"
        };
    }
};