using Core.Modules.Shared.Domain.Contracts.Services;
using Core.Modules.Stock.Domain.Constants;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.CreateBrand;

public record CreateBrandCommandResponse
{
    public string Resource { get; init; }
    
    private CreateBrandCommandResponse()
    {}

    public static CreateBrandCommandResponse Respond(string resourcePath, IAppConfigService configService)
    {
        var url = configService.GetEnvironmentVariable(StockModuleUrls.AdministrativeDashboardEnvironmentVariable);
        return new CreateBrandCommandResponse
        {
            Resource = $"{url}/{resourcePath}"
        };
    }
}