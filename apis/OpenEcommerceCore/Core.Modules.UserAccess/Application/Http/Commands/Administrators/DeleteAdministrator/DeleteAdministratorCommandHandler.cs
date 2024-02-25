using System.Net;
using Core.Modules.Shared.Domain.Contracts.Services;
using Core.Modules.Shared.Domain.ResultObjects;
using Core.Modules.UserAccess.Domain.Constants;
using Core.Modules.UserAccess.Domain.Contracts.Contexts;
using Core.Modules.UserAccess.Domain.Contracts.DynamicData.Resolvers;
using Core.Modules.UserAccess.Domain.Contracts.Http.Commands.Administrators.DeleteAdministrator;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.UserAccess.Application.Http.Commands.Administrators.DeleteAdministrator;

internal class DeleteAdministratorCommandHandler : IDeleteAdministratorCommandHandler
{
    private readonly IUserAccessContext _dbContext;
    private readonly IAppConfigService _configService;
    private readonly ICurrentCollaboratorAsyncResolver _currentCollaborator;
    
    public DeleteAdministratorCommandHandler(IUserAccessContext dbContext, IAppConfigService configService, ICurrentCollaboratorAsyncResolver currentCollaborator)
    {
        _dbContext = dbContext;
        _configService = configService;
        _currentCollaborator = currentCollaborator;
    }

    public async Task<DeleteResult> Handle(DeleteAdministratorCommand request, CancellationToken cancellationToken)
    {
        var administrator = await _dbContext.Collaborators
            .FirstOrDefaultAsync(c => c.Id == request.Id && !c.Deleted && c.IsAdmin, cancellationToken);

        if (administrator is null)
        {
            return DeleteResult.CouldNotDelete("Administrator not found", HttpStatusCode.NotFound);
        }
        
        administrator.Delete();
        await _dbContext.SaveChangesAsync(cancellationToken);

        var currentCollaborator = await _currentCollaborator.ResolveAsync();

        var administrativeFrontendUrl = _configService.GetEnvironmentVariable(ModuleUrls.AdministrativeDashboardFrontendUrl);
       
        if (administrator.Id == currentCollaborator.Id)
        {
            return DeleteResult.Delete($"{administrativeFrontendUrl}/login");
        }
        
        return DeleteResult.Delete($"{administrativeFrontendUrl}/administrators");
    }
}