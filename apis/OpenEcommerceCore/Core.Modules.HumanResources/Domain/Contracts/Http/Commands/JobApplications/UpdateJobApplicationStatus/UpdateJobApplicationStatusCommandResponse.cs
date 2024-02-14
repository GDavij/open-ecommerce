using Core.Modules.HumanResources.Domain.Constants;
using Core.Modules.Shared.Domain.Contracts.Services;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Commands.JobApplications.UpdateJobApplicationStatus;

public record UpdateJobApplicationStatusCommandResponse(string Resource)
{
    public static UpdateJobApplicationStatusCommandResponse Respond(Guid jobApplicationId, IAppConfigService configService)
    => new UpdateJobApplicationStatusCommandResponse($"{configService.GetEnvironmentVariable(ModuleUrls.AdministrativeDashboardFrontendUrl)}/job-applications/{jobApplicationId}");
}