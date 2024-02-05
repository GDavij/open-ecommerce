using Core.Modules.HumanResources.Domain.Constants;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.Contracts.AddContracts;
using Core.Modules.Shared.Domain.Contracts.Services;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Commands.Contracts.AddWorkHourToContributionYear;

public record AddWorkHourToContributionYearCommandResponse
{
    public string Resource { get; init; }
    
    public static AddWorkHourToContributionYearCommandResponse Respond(Guid contractId, IAppConfigService configService)
    {
        var baseFrontendUrl = configService.GetEnvironmentVariable(ModuleUrls.AdministrativeDashboardFrontendUrl);
        return new AddWorkHourToContributionYearCommandResponse
        { 
            Resource = $"{baseFrontendUrl}/contracts/{contractId}"
        };
    }
}