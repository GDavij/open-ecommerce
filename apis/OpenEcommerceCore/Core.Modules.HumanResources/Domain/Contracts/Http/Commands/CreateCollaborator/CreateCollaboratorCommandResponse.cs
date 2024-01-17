using Core.Modules.HumanResources.Domain.Constants;
using Core.Modules.Shared.Domain.Contracts.Services;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Commands.CreateCollaborator;

public record CreateCollaboratorCommandResponse
{
    public string Resource { get; init; }
    
    private CreateCollaboratorCommandResponse()
    {}

    public static CreateCollaboratorCommandResponse Respond(IAppConfigService configService)
    {
        var baseFrontendUrl = configService.GetEnvironmentVariable(HumanResourcesModuleUrls.AdministrativeDashboardFrontendUrl);
        return new CreateCollaboratorCommandResponse
        {
            Resource = $"{baseFrontendUrl}/collaborators"
        };
    }
};