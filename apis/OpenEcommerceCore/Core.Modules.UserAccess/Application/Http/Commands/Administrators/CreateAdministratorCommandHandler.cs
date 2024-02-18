using System.Net;
using Core.Modules.Shared.Domain.BusinessHierarchy;
using Core.Modules.Shared.Domain.Contracts.Services;
using Core.Modules.Shared.Domain.ResultObjects;
using Core.Modules.UserAccess.Domain.Constants;
using Core.Modules.UserAccess.Domain.Contracts.Contexts;
using Core.Modules.UserAccess.Domain.Contracts.Http.Commands.Administrators.CreateAdministrator;
using Core.Modules.UserAccess.Domain.Contracts.Providers;
using Core.Modules.UserAccess.Domain.Contracts.Services;
using Core.Modules.UserAccess.Domain.Entities;
using Core.Modules.UserAccess.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.UserAccess.Application.Http.Commands.Administrators;

internal class CreateAdministratorCommandHandler : ICreateAdministratorCommandHandler
{
    private readonly IUserAccessContext _dbContext;
    private readonly ISecurityService _securityService;
    private readonly IAppConfigService _configService;
    private readonly IUserAccessDateTimeProvider _dateTimeProvider;
    
    public CreateAdministratorCommandHandler(IUserAccessContext dbContext, ISecurityService securityService, IAppConfigService configService, IUserAccessDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _securityService = securityService;
        _configService = configService;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<CreationResult> Handle(CreateAdministratorCommand request, CancellationToken cancellationToken)
    {
        var existsAdmin = await _dbContext.Collaborators.AnyAsync(c => c.IsAdmin, cancellationToken); 
        if (existsAdmin && request.Authorization == null)
        {
            return CreationResult.CouldNotCreate("Already exists administrator and no token was found", HttpStatusCode.Unauthorized);
        }
        
        if (existsAdmin && request.Authorization != null)
        {
            var encodedToken = request.Authorization.Split(' ')[1];
            var isTokenValid = _securityService.TryParseEncodedToken(encodedToken, out Token token);
            if (!isTokenValid)
            {
                return CreationResult.CouldNotCreate("Token is invalid", HttpStatusCode.BadRequest);
            }

            
            long tokenLastingTime = token.Exp - _dateTimeProvider.UtcNowOffset.ToUnixTimeSeconds();
            if (tokenLastingTime <= 0)
            {
                return CreationResult.CouldNotCreate("Token is expired", HttpStatusCode.BadRequest);
            }

            var tokenCollaborator = await _dbContext.Collaborators.FirstOrDefaultAsync(c => c.Id == token.Id, cancellationToken);
            if (tokenCollaborator is null)
            {
                return CreationResult.CouldNotCreate("Token is invalid", HttpStatusCode.BadRequest);
            }
            byte[] tokenPassword = Convert.FromBase64String(token.Password);
            if (!tokenCollaborator.IsAdmin || !tokenCollaborator.Password.SequenceEqual(tokenPassword))
            {
                return CreationResult.CouldNotCreate("Token is invalid", HttpStatusCode.Forbidden);
            }
        }

        var collaboratorAlreadyExists = await _dbContext.Collaborators.AnyAsync(c => c.Email == request.Email);
        if (collaboratorAlreadyExists)
        {
            return CreationResult.CouldNotCreate($"Found an existing collaborator with email {request.Email}", HttpStatusCode.Conflict);
        }

        var securityKey = _securityService.GenerateSecurityKey();
        var derivedPassword = await _securityService.DerivePassword(request.Password, securityKey, cancellationToken);

        var collaboratorId = Guid.NewGuid();
        var collaborator = Collaborator.Create(
            collaboratorId,
            collaboratorId,
            request.Email,
            derivedPassword,
            securityKey,
            true,
            new List<ECollaboratorSector>());

        _dbContext.Collaborators.Add(collaborator);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return CreationResult.Create($"{_configService.GetEnvironmentVariable(ModuleUrls.AdministrativeDashboardFrontendUrl)}/administrators");
    }
}