using Core.Modules.Shared.Domain.Contracts.Services;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.CreateBrand;

public record CreateBrandCommandResponse
{
    public string Resource { get; init; }
    
    private CreateBrandCommandResponse()
    {}

    public static CreateBrandCommandResponse Respond(string resourcePath, IAppConfigService configService)
    {
        var url = configService.GetEnvironmentVariable("StockModule:AdministrativeDashboardBaseUrl");
        return new CreateBrandCommandResponse
        {
            Resource = $"{url}/{resourcePath}"
        };
    }
}