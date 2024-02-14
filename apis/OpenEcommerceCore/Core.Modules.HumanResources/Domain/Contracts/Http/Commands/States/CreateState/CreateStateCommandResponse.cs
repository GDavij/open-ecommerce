using Core.Modules.HumanResources.Domain.Constants;
using Core.Modules.Shared.Domain.Contracts.Services;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Commands.States.CreateState;

public record CreateStateCommandResponse(string Resource)
{
   public static CreateStateCommandResponse Respond(Guid stateId, IAppConfigService configService) 
      => new CreateStateCommandResponse($"{configService.GetEnvironmentVariable(ModuleUrls.AdministrativeDashboardFrontendUrl)}/states/{stateId}");
};