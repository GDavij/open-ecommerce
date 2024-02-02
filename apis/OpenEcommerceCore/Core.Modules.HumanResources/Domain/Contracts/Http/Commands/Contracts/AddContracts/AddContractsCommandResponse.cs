using Core.Modules.HumanResources.Domain.Constants;
using Core.Modules.Shared.Domain.Contracts.Services;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Commands.Contracts.AddContracts;

public record AddContractsCommandResponse
{
    public string Resource { get; init; }
    
    private AddContractsCommandResponse()
    {}

    public static AddContractsCommandResponse Respond(Guid collaboratorId, IAppConfigService configService)
    {
        var baseFrontendUrl = configService.GetEnvironmentVariable(ModuleUrls.AdministrativeDashboardFrontendUrl);
        return new AddContractsCommandResponse
        {
            Resource = $"{baseFrontendUrl}/collaborators/{collaboratorId}"
        };
    }
};