using System.Net;
using Core.Modules.Shared.Domain.Contracts.Services;
using Core.Modules.Shared.Domain.ResultObjects;
using Core.Modules.UserAccess.Domain.Constants;
using Core.Modules.UserAccess.Domain.Contracts.Contexts;
using Core.Modules.UserAccess.Domain.Contracts.DynamicData.Resolvers;
using Core.Modules.UserAccess.Domain.Contracts.Http.Commands.Administrators.UpdateAdministrator;
using Core.Modules.UserAccess.Domain.Contracts.Providers;
using Core.Modules.UserAccess.Domain.Contracts.Services;
using Core.Modules.UserAccess.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.UserAccess.Application.Http.Commands.Administrators.UpdateAdministrator;

internal class UpdateAdministratorCommandHandler : IUpdateAdministratorCommandHandler
{
    private readonly IUserAccessContext _dbContext;
    private readonly ICurrentCollaboratorAsyncResolver _currentCollaborator;
    private readonly ISecurityService _securityService;
    private readonly IAppConfigService _configService;
    
    public UpdateAdministratorCommandHandler(
        IUserAccessContext dbContext,
        ICurrentCollaboratorAsyncResolver currentCollaborator,
        ISecurityService securityService,
        IUserAccessDateTimeProvider dateTimeProvider,
        IAppConfigService configService)
    {
        _dbContext = dbContext;
        _currentCollaborator = currentCollaborator;
        _securityService = securityService;
        _configService = configService;
    }

    public async Task<UpdateResult> Handle(UpdateAdministratorCommand request, CancellationToken cancellationToken)
    {
    
        bool isEmailInUse = await _dbContext.Collaborators.AnyAsync(c => c.Email == request.Email && c.Id != request.Id, cancellationToken);
        if (isEmailInUse)
        {
            return UpdateResult.CouldNotUpdate("Email is already in use", HttpStatusCode.Conflict);
        }

        var administrator = await _dbContext.Collaborators
            .FirstOrDefaultAsync(c => c.Id == request.Id && c.IsAdmin, cancellationToken);

        if (administrator is null)
        {
            return UpdateResult.CouldNotUpdate("Administrator could not be found", HttpStatusCode.NotFound);
        }

        administrator.Email = request.Email;
        if (request.Password != null)
        {
            var derivedPassword = await _securityService.DerivePassword(request.Password, administrator.SecurityKey, cancellationToken);
            administrator.Password = derivedPassword;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        return UpdateResult.Update($"{_configService.GetEnvironmentVariable(ModuleUrls.AdministrativeDashboardFrontendUrl)}/administrators");
    }
}