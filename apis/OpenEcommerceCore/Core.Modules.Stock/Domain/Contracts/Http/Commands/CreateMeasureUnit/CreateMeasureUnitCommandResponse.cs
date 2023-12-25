using Core.Modules.Shared.Domain.Contracts.Services;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.CreateMeasureUnit;

public record CreateMeasureUnitCommandResponse 
{
    public string Resource { get; init; }
    
    private CreateMeasureUnitCommandResponse()
    {}
    
    public static CreateMeasureUnitCommandResponse Respond(string resourcePath, IAppConfigService configService)
    {
        var url = configService.GetEnvironmentVariable("StockModule:AdministrativeDashboardBaseUrl");
        return new CreateMeasureUnitCommandResponse
        {
            Resource = $"{url}/{resourcePath}"
        };
    }
}