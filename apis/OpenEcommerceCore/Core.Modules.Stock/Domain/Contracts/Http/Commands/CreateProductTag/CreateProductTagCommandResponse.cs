using Core.Modules.Shared.Domain.Contracts.Services;
using Core.Modules.Stock.Domain.Constants;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.CreateProductTag;

public record CreateProductTagCommandResponse
{
    public string Resource { get; init; }
    
    private CreateProductTagCommandResponse()
    {}

    public static CreateProductTagCommandResponse Respond(string resourcePath, IAppConfigService configService)
    {
        var url = configService.GetEnvironmentVariable(StockModuleUrls.AdministrativeDashboardEnvironmentVariable);
        return new CreateProductTagCommandResponse
        {
            Resource = $"{url}/{resourcePath}"
        };
    }
}