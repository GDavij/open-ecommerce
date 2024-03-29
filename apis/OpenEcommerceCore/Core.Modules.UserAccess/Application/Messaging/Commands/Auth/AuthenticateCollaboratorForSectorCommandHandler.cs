using Core.Modules.Shared.Domain.Models;
using Core.Modules.Shared.Domain.ResultObjects;
using Core.Modules.Shared.Messaging.Commands.UserAccess;
using Core.Modules.Shared.Messaging.Commands.UserAccess.Auth;
using Core.Modules.UserAccess.Domain.Contracts.Contexts;
using Core.Modules.UserAccess.Domain.Contracts.Messaging.Commands.Auth;
using Core.Modules.UserAccess.Domain.Contracts.Providers;
using Core.Modules.UserAccess.Domain.Contracts.Services;
using Core.Modules.UserAccess.Domain.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.UserAccess.Application.Messaging.Commands.Auth;

internal class AuthenticateCollaboratorForSectorCommandHandler : IAuthenticateCollaboratorForSectorCommandHandler
{
    private readonly ISecurityService _securityService;
    private readonly IUserAccessContext _dbContext;
    private readonly IUserAccessDateTimeProvider _userAccessDateTimeProvider;
    
    public AuthenticateCollaboratorForSectorCommandHandler (ISecurityService securityService, IUserAccessContext dbContext, IUserAccessDateTimeProvider userAccessDateTimeProvider)
    {
        _userAccessDateTimeProvider = userAccessDateTimeProvider;
        _dbContext = dbContext;
        _securityService = securityService;
    }
    
    public async Task Consume(ConsumeContext<AuthenticateCollaboratorForSectorCommand> context)
    {
        Token token;
        bool isValidToken = _securityService.TryParseEncodedToken(context.Message.EncodedToken, out token);
       
        if (!isValidToken)
        {
            await context.RespondAsync(AuthenticationResult.NotAuthenticated());
            return;
        }
        
        if (token.Type != ETokenType.Collaborator)
        {
            await context.RespondAsync(AuthenticationResult.NotAuthenticated());
            return;
        }
        
        long tokenLastingTime = token.Exp - _userAccessDateTimeProvider.UtcNowOffset.ToUnixTimeSeconds();
        if (tokenLastingTime <= 0)
        {
            await context.RespondAsync(AuthenticationResult.NotAuthenticated());
            return;
        }

        var existentCollaborator = await _dbContext.Collaborators
            .Where(c => c.Id == token.Id && c.Deleted == false)
            .FirstOrDefaultAsync(c => c.Sectors.Contains(context.Message.Sector) || c.IsAdmin);

        if (existentCollaborator is null)
        {
            await context.RespondAsync(AuthenticationResult.NotAuthenticated());
            return;
        }
        
        byte[] password = Convert.FromBase64String(token.Password);

        bool isPasswordValid = existentCollaborator.Password.SequenceEqual(password);
        if (!isPasswordValid)
        {
            await context.RespondAsync(AuthenticationResult.NotAuthenticated());
            return;
        }
        
        Identity identity = Identity.Create(existentCollaborator.CollaboratorModuleId!.Value);
        await context.RespondAsync(AuthenticationResult.IsAuthenticatedWithIdentity(identity));
    }
}