using Core.Modules.HumanResources.Domain.Contracts.Context;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.JobApplications.UpdateJobApplicationStatus;
using Core.Modules.HumanResources.Domain.Exceptions.JobApplication;
using Core.Modules.Shared.Domain.Contracts.Services;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.HumanResources.Application.Http.Commands.JobApplications.UpdateJobApplicationStatus;

internal class UpdateJobApplicationStatusCommandHandler : IUpdateJobApplicationStatusCommandHandler
{
    private readonly IHumanResourcesContext _dbContext;
    private readonly IAppConfigService _configService;

    public UpdateJobApplicationStatusCommandHandler(IHumanResourcesContext dbContext, IAppConfigService configService)
    {
        _dbContext = dbContext;
        _configService = configService;
    }

    public async Task<UpdateJobApplicationStatusCommandResponse> Handle(UpdateJobApplicationStatusCommand request, CancellationToken cancellationToken)
    {
        var jobApplication = await _dbContext.JobApplications.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (jobApplication is null)
        {
            throw new InvalidJobApplicationException(request.Id);
        }

        jobApplication.ProcessStep = request.ProcessStatus;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return UpdateJobApplicationStatusCommandResponse.Respond(jobApplication.Id, _configService);
    }
}