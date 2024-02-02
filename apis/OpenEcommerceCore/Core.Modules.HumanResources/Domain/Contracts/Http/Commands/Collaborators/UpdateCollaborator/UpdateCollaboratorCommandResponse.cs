using Core.Modules.HumanResources.Domain.Constants;
using Core.Modules.Shared.Domain.Contracts.Services;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Commands.Collaborators.UpdateCollaborator;

public record UpdateCollaboratorCommandResponse
{
     public string Resource { get; init; }
        
     private UpdateCollaboratorCommandResponse()
     {}
    
     public static UpdateCollaboratorCommandResponse Respond(Guid collaboratorId, IAppConfigService configService)
     {
         var baseFrontendUrl = configService.GetEnvironmentVariable(ModuleUrls.AdministrativeDashboardFrontendUrl);
         return new UpdateCollaboratorCommandResponse
         {
             Resource = $"{baseFrontendUrl}/collaborators/{collaboratorId}"
         };
     }
}